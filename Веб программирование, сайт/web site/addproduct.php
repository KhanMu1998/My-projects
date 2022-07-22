<?php


// Соединямся с БД
$link=mysqli_connect("localhost", "root", "", "stu8");

if (isset($_COOKIE["gurman_user_role"])) {
    if ($_COOKIE["gurman_user_role"]=='admin') {

   
    {   $sql = "INSERT INTO products (name, price, type, description, availability, image, life) 
                VALUES ('new', 0, 'icecream', 'new', 0, 'new.png', 0)";
        
        if (mysqli_query($link, $sql)) {
          //echo "New record created successfully";
          header("Location: main_page.php"); 
        } else {
          echo "Error: " . $sql . "<br>" . mysqli_error($link);
        }
    }
}
}

    mysqli_close($link);


?>