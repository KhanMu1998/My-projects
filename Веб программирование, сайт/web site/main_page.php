<!--<!DOCTYPE html>-->
<html>
<head>
   <meta charset="utf-8">
    <title>Гурман</title>
<link rel="stylesheet" type="text/css" href="css/style1.css">
</head>
<body>
   
   <script src="js/jquery-3.5.1.min.js"></script>
<!--    <script src="js/admin.js"></script> -->
   <script src="js/main.js"></script>

<div id="container">
	<div id="header">
      <div id="header_1">
         <a href="main_page.php"><img id="name_logo" src="interface_img/name_logo.jpg"></a>
      </div>

      <div class="two"><a href="#">Скидки</a></div>
      <div class="two"><a href="#">Кэшбэк</a></div>
      <div class="two"><a href="#">Купоны</a>
      <div class="dropdown-content">
         <a href="#">Получить</a>
         <a href="#">Использовать</a>
         <a href="#">Узнать больше</a>
         </div>
      </div>            

      <div class="d1">
         <form id="form_search">
         <input id="search_button" type="text" placeholder="Поиск товаров">
         <button type="submit" onclick = "search()"><p>Найти</p></button>
         </form>
       </div>

       <div class="header_3" onclick="open_cart()"> <!-- onclick="open_cart()" -->
         <a href="#"><img class="cart1" src="interface_img/cart.png"></a>
      </div>
       <div class="header_2" onclick="document.getElementById('id01').style.display='block'" >
         <a href="#"><img class="login" src="interface_img/login.png"></a>
      </div>
      
   </div>
		 
	<div id="navigation">
      <div class="slider">

         <div class="sli sli1"><img src="slider_img/slider_right.jpg"></div>
         <div class="sli sli2"><img src="slider_img/slider_center1.jpg"></div>
         <div class="sli sli3"><img src="slider_img/slider_left.jpg"></div>
       </div>
	</div>
		 
	<div id="sidebar">
      <table id="side">
         <tr>
            <td><img src="types_img/Мороженое.orig"></td>
            <td><a href="#icecream" onclick = "refresh('icecream')"><p>Мороженое</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/хлеб.orig"></td>
            <td><a href="#bread" onclick = "refresh('bread')"><p>Хлеб</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/молоко.orig"></td>
            <td><a href="#milk" onclick = "refresh('milk')"><p>Молоко</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/рыба.orig"></td>
            <td><a href="#fish" onclick = "refresh('fish')"><p>Рыба</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/Фрукты.orig"></td>
            <td><a href="#fruits" onclick = "refresh('fruits')"><p>Фрукты</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/Чай.orig"></td>
            <td><a href="#Coffee" onclick = "refresh('Coffee')"><p>Кофе</p></a></td>
         </tr>
         <tr>
            <td><img src="types_img/Орехи.orig"></td>
            <td><a href="#nuts" onclick = "refresh('nuts')"><p>Орехи</p></a></td>
         </tr>
      </table>
   <script>
      function refresh(x) {
         window.location.href = 'main_page.php#' + x;
         window.location.reload(); 
      }

       function search() {
         inp = document.getElementById("search_button").value;
         console.log(inp);
         window.location.href = 'main_page.php#' + inp;
         init_search();
         window.location.href = 'main_page.php';
      } 
      
      function open_cart(){
         init_cart();
         document.getElementById('cartcontainer').style.display='flex';

      }

      function load_file(){
         document.getElementById('id03').style.display='block'
      }

      function close_load_file(){
         document.getElementById('id03').style.display='none'
      }

      </script>
	</div>
		 
	<div id="content">
   <div id="content_inner"></div>
	</div>
		 
	<div id="clear">
		 
	</div>
							   
	<div id="footer">
      <div id="about">
         <p>
            © 2001–2020 ООО «Gourmet»
            Все права защищены. «Gourmet» является 
            зарегистрированным товарным знаком 
            Gourmet Holding SA.
         </p>
      </div>
      
      <div id="phone">
         <p>8 850 800-41-00</p>
         <p id="phone_help">Круглосуточная поддержка клиентов</p>
      </div>
	</div>
</div>



<?php 

if (isset($_COOKIE["gurman_user_login"])) 
{
   echo '
   <div id="id01" class="modal">
     
     <form class="modal-content animate" action="logout.php" method="POST" >
       <div class="login_imgcontainer">
         <span onclick="document.getElementById(\'id01\').style.display=\'none\'" class="close" title="Close Modal">×</span>
         <img src="interface_img/img_avatar2.png" alt="Avatar" class="avatar">
       </div>
   
       <div class="login_container">
         <p><b>Имя текущего пользователя: ';print( $_COOKIE["gurman_user_name"]); echo'</b></p>
         <p><b>Логин: ';print( $_COOKIE["gurman_user_login"]); echo'</b></p>';

         if (isset($_COOKIE["gurman_user_role"])) {
            if ($_COOKIE["gurman_user_role"]=='admin') {
            echo '<a href="addproduct.php">Добавить товар</a>';
            }
         }
         echo'         
         <button type="submit">Выйти</button>
         
       </div>
   
     </form>
   </div>
   ';
/*    if ($_COOKIE["gurman_user_role"]=="admin") {
      echo ' 
      <div id="id03" class="uppermodal">
        
         <form enctype="multipart/form-data" action="cartedit.php" method="post">
            
            <span onclick="document.getElementById(\'id03\').style.display=\'none\'" class="close" title="Close Modal">×</span>
            
            <input type="hidden" name="MAX_FILE_SIZE" value="1500000">
            <input name="userfile" type="file">
            <input type="submit" value="Сохранить">
         </form>
      </div>
      ';      
   } */
}
else 

   {
      echo '
      <div id="id01" class="modal">
        
        <form class="modal-content animate" action="login.php" method="POST" >
          <div class="login_imgcontainer">
            <span onclick="document.getElementById(\'id01\').style.display=\'none\'" class="close" title="Close Modal">×</span>
            <img src="interface_img/img_avatar2.png" alt="Avatar" class="avatar">
          </div>
      
          <div class="login_container">
            <label for="uname"><b>Логин</b></label>
            <input type="text" placeholder="Введите имя пользователя" name="uname" required >
      
            <label for="psw"><b>Пароль</b></label>
            <input type="password" placeholder="Введите пароль" name="psw" required>
              
            <button type="submit">Войти</button>
            
          </div>
      
          <div class="login_container" style="background-color:#f1f1f1">
            <button type="button" onclick="document.getElementById(\'id01\').style.display=\'none\'" class="cancelbtn">Отмена</button>
            <span class="psw"><a href="#">Забыли пароль?</a></span>
          </div>
        </form>
      </div>
      '; }


      if ($_COOKIE["gurman_prod_key_id"]=="prod1") {
         echo 'Все есть';
      }
?>


<div id="prod"></div>
<div id="cartcontainer"></div>

<script src="js/script.js"></script>

</body>
</html>