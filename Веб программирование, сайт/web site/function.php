<?php


function connect(){
    $conn = mysqli_connect("localhost", "root", "", "stu8");
    if (!$conn) {
        die("Connection failed: " . mysqli_connect_error());
    }
    return $conn;
}

function init(){
    //вывожу список товаров
    $type = $_POST['type'];
    if ($type == "") 
        {$type = "icecream";}
    $conn = connect();
    if (isset($_COOKIE["gurman_user_role"])) {
        if ($_COOKIE["gurman_user_role"]=='admin') {
            $sql = "SELECT id,name,price,type,description,availability,image,life FROM products WHERE type = '$type'";
        }
        else   {
            $sql = "SELECT id,name,price,type,description,availability,image,life FROM products WHERE type = '$type' and availability>0";  
        } 
    }
    else {
        $sql = "SELECT id,name,price,type,description,availability,image,life FROM products WHERE type = '$type' and availability>0";  
    }


    $result = mysqli_query($conn, $sql);

    if (mysqli_num_rows($result) > 0) {
        $out = array();
        while($row = mysqli_fetch_assoc($result)) {
            $out[$row["id"]] = $row;
        }
        echo json_encode($out);
    } else {
        echo "0";
    }
    mysqli_close($conn);
}

function init_search(){
    //вывожу список товаров
    $type = $_POST['type'];    

    $conn = connect();
    $sql = "SELECT * FROM products WHERE upper(name) LIKE upper('%$type%')";
    $result = mysqli_query($conn, $sql);

    if (mysqli_num_rows($result) > 0) {
        $out = array();
        while($row = mysqli_fetch_assoc($result)) {
            $out[$row["id"]] = $row;
        }
        echo json_encode($out);
    } else {
        echo "0";
    }
    mysqli_close($conn);
}

function init_cart(){
    $user   = $_COOKIE["gurman_user_id"];

    if (isset($user)) {

    $conn = connect();
    $sql = "SELECT c.c_id, c.c_count, p.id, p.name, p.price, p.image FROM cart c join products p on p.id=c.p_id WHERE c.u_id=".$user;
    $result = mysqli_query($conn, $sql);
    
    $i=0;
    if (mysqli_num_rows($result) > 0) {
        $out = array();
        while($row = mysqli_fetch_assoc($result)) {
            $i=$i+1;
            $out[$i] = $row;
        }
        echo json_encode($out);
    } else {
        echo "0";
    }
    mysqli_close($conn);
} 

}

function clear_cart(){
    $user   = $_COOKIE["gurman_user_id"];

    if (isset($user)) {

    $conn = connect();
    $sql = "DELETE FROM cart WHERE u_id=".$user;
    $result = mysqli_query($conn, $sql);
    }
    $out = array();
}

?>