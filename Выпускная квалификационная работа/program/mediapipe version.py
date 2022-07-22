import cv2
import mediapipe as mp
import numpy as np
import time
import math
import playsound as ps

# Подключаем детектор лица и лицевых точек
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh(min_detection_confidence=0.5, min_tracking_confidence=0.5)
# Подключаем видео с веб-камеры
cap = cv2.VideoCapture(0)
# Создаем таймер для предупреждающего сигнала
alert_timer = time.time()
# Создаем переменные для подсчета кадров в секунду
ten_frames_time = np.zeros(100)
while cap.isOpened():
    # Фиксируем время начала работы программы
    start_time = time.time()
    # Читаем кадр от веб-камеры
    success, image = cap.read()
    image_sharp = image
    # Получаем размеры изображения
    image_height, image_width, image_chanels = image.shape    
    # Получаем результат поиска лицевых точек
    results = face_mesh.process(image) 
    if results.multi_face_landmarks:
        # Получаем 2D координаты 6 ключевых лицевых точек
        face = results.multi_face_landmarks[0]        
        face_2d = np.array([
                (face.landmark[4].x, face.landmark[4].y),       # Кончик носа
                (face.landmark[199].x, face.landmark[199].y),   # Подбородок
                (face.landmark[33].x, face.landmark[33].y),     # Левый угол левого глаза
                (face.landmark[263].x, face.landmark[263].y),   # Правый угол правого глаза
                (face.landmark[61].x, face.landmark[61].y),     # Левый угол рта
                (face.landmark[291].x, face.landmark[291].y)    # Правый угол рта
            ], dtype="double")
        
        for i, item in enumerate(face_2d):
            face_2d[i][0] = face_2d[i][0] * image_width
            face_2d[i][1] = face_2d[i][1] * image_height
        
        # Создаем усредненную модель лица
        face_3d = np.array([
                        (0.0, 0.0, 0.0),             # Кончик носа
                        (0.0, -330.0, -65.0),        # Подбородок
                        (-225.0, 170.0, -135.0),     # Левый угол левого глаза
                        (225.0, 170.0, -135.0),      # Правый угол правого глаза
                        (-150.0, -150.0, -125.0),    # Левый угол рта
                        (150.0, -150.0, -125.0)      # Правый угол рта                         
                        ])
        # Создаем вектор, указывающий направление поворота лица        
        nose_3d = np.array([(0.0, 0.0, 1000.0)])

        # Приближенное фокусное расстояние камеры
        focal_length = 1 * image_width
        # Матрица внутренних параметров камеры
        cam_matrix = np.array([ [focal_length, 0, image_height / 2],
                                [0, focal_length, image_width / 2],
                                [0, 0, 1]])
        # Приближенная матрица искажений
        dist_matrix = np.zeros((4, 1), dtype=np.float64)

        # Решаем задачу PnP
        success, rot_vec, trans_vec = cv2.solvePnP(face_3d, face_2d, cam_matrix, dist_matrix)
        
        # Вычисляем углы Эйлера
        R, jac = cv2.Rodrigues(rot_vec)
        sy = math.sqrt(R[0,0] * R[0,0] +  R[1,0] * R[1,0])
        my = 60*math.atan2(R[2,1] , R[2,2])
        if my > 0:  y = 188 - int(my)
        else: y = -188 - int(my)
        x = int(60*math.atan2(-R[2,0], sy)) - 8
        z = int(60*math.atan2(R[1,0], R[0,0]))
        new_euler_angle_str = 'Y:{}, X:{}, Z:{}'.format(y, x, z)
        
        # Определяем направление поворота головы
        if y > 14:
            direction = "Вверх"
        elif y < -14:
            direction = "Вниз"
        elif x < -20:
            direction = "Влево"
        elif x > 20:
            direction = "Вправо"
        else:
            direction = "Вперед"
            # Обнуляем таймер, если смотрим прямо
            alert_timer = time.time()

        max_time = 5
        if  (time.time() - alert_timer) < max_time:
            # Находим оставшееся до предупреждения время
            alert = "Время до предупреждения: " + str(int(max_time - (time.time() - alert_timer))) + " сек"
        else:
            # Воспроизводим звуковое предупреждение
            alert = "ВНИМАНИЕ"
            ps.playsound('horn.wav')
        
        # Находим затраченное на вычисление время
        used_time = time.time() - start_time
        # Находим число кадров в секунду
        ten_frames_time[:-1] = ten_frames_time[1:]
        ten_frames_time[-1] = used_time
        fps = 1 / np.average(ten_frames_time)
        #print(int(1/used_time))

        #Отображаем направление поворота головы
        nose_3d_projection, jacobian = cv2.projectPoints(nose_3d, rot_vec, trans_vec, cam_matrix, dist_matrix)
        p1 = (int(face_2d[0][0]), int(face_2d[0][1]))
        p2 = (int(nose_3d_projection[0][0][0]), int(nose_3d_projection[0][0][1]))
        # Выводим лицевые точки
        for p in face_2d:
            cv2.circle(image, (int(p[0]), int(p[1])), 3, (0,0,0), -1, cv2.LINE_AA)
            cv2.circle(image, (int(p[0]), int(p[1])), 2, (0,0,255), -1, cv2.LINE_AA)
        # Выводим линию от носа
        cv2.line(image, p1, p2, (0, 0, 0), 4, cv2.LINE_AA)
        cv2.line(image, p1, p2, (255, 255, 0), 2, cv2.LINE_AA)

        image_sharp = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)        
        image_sharp = cv2.cvtColor(image_sharp, cv2.COLOR_GRAY2BGR)
        kernel = np.array([[-1,-1,-1], [-1,9.6,-1], [-1,-1,-1]])
        image_sharp = cv2.filter2D(image_sharp, -1, kernel)
        # Выводим таймер до предупреждения
        cv2.putText( image_sharp, alert, (10, 30), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA )
        cv2.putText( image_sharp, alert, (10, 30), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим углы поворота
        cv2.putText( image_sharp, new_euler_angle_str, (10, 60), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA )
        cv2.putText( image_sharp, new_euler_angle_str, (10, 60), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим направление поворота головы
        cv2.putText( image_sharp, direction, (10, 90), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA ) 
        cv2.putText( image_sharp, direction, (10, 90), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим число кадров в секунду
        cv2.putText(image_sharp, f'FPS: {int(fps)}', (10,image_height - 10), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0, 0, 0), 5, cv2.LINE_AA)
        cv2.putText(image_sharp, f'FPS: {int(fps)}', (10,image_height - 10), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255, 255, 255), 2, cv2.LINE_AA)
       
        # Выводим таймер до предупреждения
        cv2.putText( image, alert, (10, 30), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA )
        cv2.putText( image, alert, (10, 30), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим углы поворота
        cv2.putText( image, new_euler_angle_str, (10, 60), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA )
        cv2.putText( image, new_euler_angle_str, (10, 60), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим направление поворота головы
        cv2.putText( image, direction, (10, 90), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0,0,0), 5, cv2.LINE_AA ) 
        cv2.putText( image, direction, (10, 90), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255,255,255), 2, cv2.LINE_AA )
        # Выводим число кадров в секунду
        cv2.putText(image, f'FPS: {int(fps)}', (10,image_height - 10), cv2.FONT_HERSHEY_COMPLEX, 0.75, (0, 0, 0), 5, cv2.LINE_AA)
        cv2.putText(image, f'FPS: {int(fps)}', (10,image_height - 10), cv2.FONT_HERSHEY_COMPLEX, 0.75, (255, 255, 255), 2, cv2.LINE_AA)
        
    cv2.imshow('Head Pose Estimation', image)
    cv2.imshow('Head Pose Estimation ', image_sharp)
    #cv2.imshow('Head Pose Estimation', np.hstack([image_sharp, image]))

    if cv2.waitKey(5) == ord(" "):
        break

cap.release()
cv2.destroyAllWindows()
