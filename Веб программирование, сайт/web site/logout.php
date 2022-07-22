<?php
  
  setcookie("gurman_user_login", '', strtotime("+30 days"));
  setcookie("gurman_user_id", '', strtotime("+30 days"));
  setcookie("gurman_user_role", '', strtotime("+30 days"));
  header("Location: main_page.php"); 
        
?>