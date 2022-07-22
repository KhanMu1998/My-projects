<?php

// Соединямся с БД
$link=mysqli_connect("localhost", "root", "", "stu8");
$prod_id = mysqli_real_escape_string($link,$_POST['prod_id']);
//$prod_img= mysqli_real_escape_string($link,$_POST['prod_img']);
$prod_name=mysqli_real_escape_string($link,$_POST['prod_name']);
$prod_price=mysqli_real_escape_string($link,$_POST['prod_price']);
$prod_life=mysqli_real_escape_string($link,$_POST['prod_life']);
$prod_av=mysqli_real_escape_string($link,$_POST['prod_av']);
$prod_desc=mysqli_real_escape_string($link,$_POST['prod_desc']);
$type=mysqli_real_escape_string($link,$_POST['prod_type']);

$uploaddir = $_SERVER['DOCUMENT_ROOT'].'/Shop/products_img/';
$uploadfile = $uploaddir.basename($_FILES['userfile']['name']);

//$result=move_uploaded_file($_FILES['userfile']['tmp_name'], $uploadfile);
$file_name = $_FILES['userfile']['name'];

$image_blob = addslashes(file_get_contents($_FILES['userfile']['tmp_name']));
$image_type = $_FILES['userfile']['type'];
//echo $image_blob ;
$result=move_uploaded_file($_FILES['userfile']['tmp_name'], $uploadfile); 

$user   = $_COOKIE["gurman_user_id"];
   
    $query = mysqli_query($link,"SELECT * FROM products WHERE id=".$prod_id);
    

    if (mysqli_num_rows($query) > 0){  
        $data= mysqli_fetch_assoc( $query );
        if ($file_name=="") {
            $sql = "UPDATE products SET name='".$prod_name."',  price = ".$prod_price.", type='".$type."',
            life = ".$prod_life.", description='".$prod_desc."', availability=".$prod_av." WHERE id=".$prod_id;
        } else {
            $sql = "UPDATE products SET name='".$prod_name."',  image='".$file_name."', price = ".$prod_price.", type='".$type."',
            life = ".$prod_life.", description='".$prod_desc."', availability=".$prod_av.", image_blob='".$image_blob."', image_type='".$image_type."' WHERE id=".$prod_id;
        }
        
        //echo $sql;
        if (mysqli_query($link, $sql)) {
        //echo "Record updated successfully";
        header("Location: main_page.php"); 
        } else {
        echo "Error updating record: " . mysqli_error($link);
        }

    }
    mysqli_close($link);


?>