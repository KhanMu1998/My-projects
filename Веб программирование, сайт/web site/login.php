<?php
// Страница авторизации

// Соединямся с БД
$link=mysqli_connect("localhost", "root", "", "stu8");
$login = mysqli_real_escape_string($link,$_POST['uname']);

    // Вытаскиваем из БД запись, у которой логин равняеться введенному
    $query = mysqli_query($link,"SELECT u_id, u_password, u_name, u_role FROM users WHERE u_login='".$login."' LIMIT 1");
    $data = mysqli_fetch_assoc($query);

    
    // Сравниваем пароли
    // if($data['u_password'] === md5(md5($_POST['password'])))
    if($data['u_password'] === $_POST['psw'])
    {   
        setcookie("gurman_user_login", $login, strtotime("+30 days"));
        setcookie("gurman_user_name", $data['u_name'], strtotime("+30 days"));
        setcookie("gurman_user_id",  $data['u_id'], strtotime("+30 days"));
        setcookie("gurman_user_role",  $data['u_role'], strtotime("+30 days"));
        session_start();
        $_SESSION["gurman_user_login"] = $login;
        header("Location: main_page.php"); 
        
        //exit();
    }
    else
    {
        print "Вы ввели неправильный логин/пароль";
    }
//}
?>