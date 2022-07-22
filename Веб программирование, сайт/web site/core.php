<?php

$action = $_POST['action'];

require_once 'function.php';

switch ($action) {
    case 'init':
        init();
        break;
    case 'search':
        init_search();
        break;   
    case 'init_cart':
        init_cart();
        break;   
    case 'clear_cart':
        clear_cart();
        break;   
}
?>