<?php

use Slim\Http\Request;
use Slim\Http\Response;

// Routes

$app->get('/[{name}]', function (Request $request, Response $response, array $args) {
    // Sample log message
    $this->logger->info("Slim-Skeleton '/' route");

    // Render index view
    return $this->renderer->render($response, 'index.phtml', $args);
});

// Routes
// Grupo de rutas para el API
$app->group('/api', function () use ($app) {
    
    $app->get('/user/{email}', 'getUser');
    $app->get('/shops/{email}', 'getUser');
    $app->get('/factions_points', 'getFactionsPoints');
    $app->put('/shops_coordinates', 'getShopsCoordinates');
    $app->put('/guilds_coordinates', 'getGuildsCoordinates');
    $app->put('/bosses_coordinates', 'getBossesCoordinates');
    $app->get('/shop/{id}', 'getShop');
    $app->get('/user_player/{email}', 'getPlayer');
    $app->put('/create', 'createUser');
    $app->put('/create_player', 'createPlayer');
    $app->put('/update_coordinates', 'updateCoordinates');
    $app->put('/login', 'userLogin');
    $app->put('/delete_item', 'deleteItem');
    $app->put('/update_attributes', 'updateAttributes');
    $app->put('/add_item', 'addItem');
    $app->put('/equip_items', 'equipItems');
    $app->put('/quests', 'getQuests');
    $app->put('/accept_quest', 'acceptQuest');
    $app->put('/accepted_quests', 'getAcceptedQuests');
    $app->put('/reward', 'getReward');
    $app->put('/progress_quest', 'progressInQuest');
    $app->put('/use_bravery_points', 'useBraveryPoints');
    $app->put('/add_bravery_point', 'addBraveryPoint');
    $app->put('/add_faction_points', 'addFactionPoints');
    $app->put('/check_notifications', 'checkNotifications');
    $app->put('/start_online_battle', 'startOnlineBattle');
    $app->put('/set_player_battle_stats', 'setPlayerBattleStats');
    $app->put('/get_player_battle_stats', 'getPlayerBattleStats');
    $app->put('/set_online_battle_command', 'setOnlineBattleCommand');
    $app->put('/get_online_battle_command', 'getOnlineBattleCommand');
    $app->put('/set_chat_line', 'setChatLine');
    $app->put('/get_chat', 'getChat');
    $app->put('/complete_quest', 'completeQuest');
});

function emailAlreadyExits($email) {
    $exits = true;
    // Comprobamos que el email no existe ya
    $sql = "SELECT * FROM user WHERE email=:email";
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql);
        $stmt->bindParam("email", $email);
        $stmt->execute();
        $num_rows = $stmt->fetchColumn();
        if($num_rows==0) {
            $exits = false;
        }  
        $db = null;        
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }

    return $exits;
}

// Comprobamos que el usuario se ha identificado
function checkUserPassword($email, $password) {
    $salt = md5($password);
    $encrypted_password = crypt($password, $salt);
    $sql = "SELECT id FROM user WHERE email=:email AND user_password=:user_password";
    $check = false;
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql);
        $stmt->bindParam("user_password", $encrypted_password);
        $stmt->bindParam("email", $email);
        $stmt->execute();
        $num_rows = $stmt->fetchColumn();
        if($num_rows==0) {
            $check = false;
        }
        else {
            $check = true;
        }
        $db = null;  
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }

    return $check;
}

// Obtenemos el id del usuario
function getUserId($email, $password) {
    $salt = md5($password);
    $encrypted_password = crypt($password, $salt);
    $sql = "SELECT id FROM user WHERE email=:email AND user_password=:user_password";
    $check = 0;
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql);
        $stmt->bindParam("user_password", $encrypted_password);
        $stmt->bindParam("email", $email);
        $stmt->execute();             
        while ($row = $stmt->fetch())
        {  
            $check = $row['id'];
        }              
        $db = null;  
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }

    return $check;
}

// Crea un usuario
function createUser($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que el email no existe ya
    $email_exits = emailAlreadyExits($data->email);

    if(!($email_exits)) {
        $salt = md5($data->user_password);
        $encrypted_password = crypt($data->user_password, $salt);

        $sql = "INSERT INTO user (user_password, email) VALUES (:user_password, :email)";
        try {
            $db = getConnection();
            $stmt = $db->prepare($sql);
            $stmt->bindParam("user_password", $encrypted_password);
            $stmt->bindParam("email", $data->email);
            $stmt->execute();
            // Devolvemos la id adquirida y los datos enviados
            $data->id = $db->lastInsertId();
            $db = null;
            echo json_encode($data);
        } catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
        }
    }

    else {
        $data->id = 0;        
        if($email_exits) {
            $data->email = "exits";
        }
        echo json_encode($data);
    }
}

// Comprueba que el usuario es valido
function userLogin($request) {
    $data = json_decode($request->getBody());

    if(checkUserPassword($data->email, $data->user_password)) {
        $array = array("correct_data" => 1, "email" => $data->email, "user_password" => $data->user_password);
        echo json_encode($array);      
    }
    else {
        $array = array("correct_data" => 0);
        echo json_encode($array);      
    }
}

// Actualiza las coordenadas del usuario y devuelve la de los usuarios cercanos
function updateCoordinates($request) {
    $data = json_decode($request->getBody());
       
    //$salt = md5($data->user_password);
    //$encrypted_password = crypt($data->user_password, $salt);
    $sql = "UPDATE user SET latitude = :latitude, longitude = :longitude, last_connection = NOW() WHERE email=:email";
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql);
        $stmt->bindParam("latitude", floatval($data->latitude));
        $stmt->bindParam("longitude", floatval($data->longitude));
        $stmt->bindParam("email", $data->email);
        $stmt->execute();       
        $db = null;           
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
    
    // se comprueba si hay jugadores cerca cuya conexion no haya caducado y se devuelven las coordenadas
    $sql = "SELECT id, latitude, longitude, last_connection FROM user WHERE email!=:email AND last_connection > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 20 SECOND)";
    $sql2 = "SELECT id, faction, name FROM player WHERE user_id=:user_id";
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql);      
        $stmt->bindParam("email", $data->email);
        $coordinates = array();
        $players_id = array();
        $players_factions = array();
        $players_names = array();
        $stmt->execute();
        while ($row = $stmt->fetch())
        {               
            if(!(empty($row['latitude'])) && !(is_null($row['latitude'])) && !(empty($row['last_connection'])) && !(is_null($row['last_connection']))) {           
                $data = strval($row['latitude']) . ", " . strval($row['longitude']);
                $coordinates[] = array('coordinates' => $data);
                $user_id = $row['id'];
                $db = getConnection();
                $second_stmt = $db->prepare($sql2);      
                $second_stmt->bindParam("user_id", $user_id);
                $second_stmt->execute();
                while($second_row = $second_stmt->fetch()) {
                    $players_id[] = array('player_id' => $second_row['id']);
                    $players_factions[] = array('player_faction' => $second_row['faction']);
                    $players_names[] = array('player_name' => $second_row['name']);
                }
            }      
        }
        $array = array("players_coordinates" => $coordinates, "players_id" => $players_id, "players_factions" => $players_factions, "players_names" => $players_names);
        $db = null;
        echo json_encode($array);      
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
}

function setChatLine($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "SELECT id FROM player WHERE user_id=:user_id";
        $sql2 = "INSERT INTO chat_line (owner, second_player_id, text, date) VALUES (:owner, :second_player_id, :text, NOW())";
        try {
            $db = getConnection();           
            $stmt = $db->prepare($sql);           
            $stmt->bindParam("user_id", $user_id);     
            $stmt->execute();                  
            while ($row = $stmt->fetch())
            {  
                $owner = $row['id'];
            }   

            $stmt = $db->prepare($sql2);           
            $stmt->bindParam("owner", $owner);
            $stmt->bindParam("second_player_id", $data->second_player_id);
            $stmt->bindParam("text", $data->line_text);
            $stmt->execute();   

            $db = null;           
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

function getChat($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "SELECT id FROM player WHERE user_id=:user_id";
        $sql2 = "SELECT * FROM chat_line WHERE (owner=:first_player_id AND second_player_id=:second_player_id) OR (owner=:second_player_id AND second_player_id=:first_player_id) ORDER BY date DESC";
        try {
            $db = getConnection();           
            $stmt = $db->prepare($sql);           
            $stmt->bindParam("user_id", $user_id);     
            $stmt->execute();                  
            while ($row = $stmt->fetch())
            {  
                $first_player_id = $row['id'];
            }   

            $stmt = $db->prepare($sql2);           
            $stmt->bindParam("first_player_id", $first_player_id);
            $stmt->bindParam("second_player_id", $data->second_player_id);
            $stmt->execute();

            $lines_owners = array();
            $lines_text = array();
            $dates = array();
            while ($row = $stmt->fetch())
            {  
                $lines_owners[] = array('owner_id' => $row['owner']);
                $lines_text[] = array('text' => $row['text']);
                $dates[] = array('date' => date("H:i:s", strtotime($row['date'])));
            }   

            $array = array("lines_owners" => $lines_owners, "lines_text" => $lines_text, "dates" => $dates);
            $db = null;
            echo json_encode($array);            
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

function getFactionsPoints($request) {
    $sql = "SELECT name, points FROM faction";
    try {
        $factions_names = array();
        $factions_points = array();
        $db = getConnection();
        $stmt = $db->prepare($sql);      
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $factions_names[] = array('name' => $row['name']);
            $factions_points[] = array('points' => $row['points']);
        }       
        $array = array("factions_names" => $factions_names, "factions_points" => $factions_points);
        $db = null;
        echo json_encode($array);  
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
}

// devuelve la posicion de las tiendas con sus identificadores
function getShopsCoordinates($request) {
    $data = json_decode($request->getBody());

    //$sql = "SELECT id, latitude, longitude FROM shop";
    $sql = "SELECT id, latitude, longitude, ( 6371 * acos(cos(radians(:latitude)) * cos(radians(latitude)) * cos(radians(longitude) - radians(:longitude)) + sin(radians(:latitude)) * sin(radians(latitude)))) AS distance FROM shop HAVING distance < 0.5 ORDER BY distance";
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql); 
        $stmt->bindParam("latitude", $data->latitude);
        $stmt->bindParam("longitude", $data->longitude);        
        $coordinates = array();
        $shops_id = array();
        $stmt->execute();
        while ($row = $stmt->fetch())
        {                       
            $data = strval($row['latitude']) . ", " . strval($row['longitude']);
            $coordinates[] = array('coordinates' => $data);
            $shops_id[] = array('id' => $row['id']);
        }
        $array = array("shops_coordinates" => $coordinates, "shops_id" => $shops_id);
        $db = null;
        echo json_encode($array);      
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
}

// devuelve la posicion de los jefes cercanos
function getBossesCoordinates($request) {
    $data = json_decode($request->getBody());

    $sql = "SELECT id, enemy_id, bravery_points_required, latitude, longitude, ( 6371 * acos(cos(radians(:latitude)) * cos(radians(latitude)) * cos(radians(longitude) - radians(:longitude)) + sin(radians(:latitude)) * sin(radians(latitude)))) AS distance FROM boss_enemy HAVING distance < 0.5 ORDER BY distance";
    $sql2 = "SELECT bravery_points FROM user WHERE email=:email";
    try {
        $email = $data->email;
        $db = getConnection();
        $stmt = $db->prepare($sql); 
        $stmt->bindParam("latitude", $data->latitude);
        $stmt->bindParam("longitude", $data->longitude);        
        $coordinates = array();
        $bosses_id = array();
        $bravery_points = array();
        $stmt->execute();
        while ($row = $stmt->fetch())
        {                       
            $data = strval($row['latitude']) . ", " . strval($row['longitude']);
            $coordinates[] = array('coordinates' => $data);
            $bosses_id[] = array('boss_id' => $row['enemy_id']);
            $bravery_points[] = array('bravery_points' => $row['bravery_points_required']);
        }

        $user_bravery_points;
        $stmt = $db->prepare($sql2);
        $stmt->bindParam("email", $email);                
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $user_bravery_points = $row['bravery_points'];            
        }       

        $array = array("bosses_coordinates" => $coordinates, "bosses_id" => $bosses_id, "bosses_bravery_points" => $bravery_points, "user_bravery_points" => $user_bravery_points);
        $db = null;
        echo json_encode($array);      
    } catch(PDOException $e) {
        $data->id = -1;
        echo $e;
        echo json_encode($data);
    }
}

// devuelve la posicion de los gremios
function getGuildsCoordinates($request) {
    $data = json_decode($request->getBody());

    $sql = "SELECT name, latitude, longitude, ( 6371 * acos(cos(radians(:latitude)) * cos(radians(latitude)) * cos(radians(longitude) - radians(:longitude)) + sin(radians(:latitude)) * sin(radians(latitude)))) AS distance FROM guild HAVING distance < 0.5 ORDER BY distance";
    try {
        $db = getConnection();
        $stmt = $db->prepare($sql); 
        $stmt->bindParam("latitude", $data->latitude);
        $stmt->bindParam("longitude", $data->longitude);        
        $coordinates = array();
        $guilds_names = array();
        $stmt->execute();
        while ($row = $stmt->fetch())
        {                       
            $data = strval($row['latitude']) . ", " . strval($row['longitude']);
            $coordinates[] = array('coordinates' => $data);
            $guilds_names[] = array('name' => $row['name']);
        }
        $array = array("guilds_coordinates" => $coordinates, "guilds_names" => $guilds_names);
        $db = null;
        echo json_encode($array);      
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
}

// devuelve los objetos de una tienda
function getShop($request) {
    $id = $request->getAttribute('id');

    $sql = "SELECT item_name FROM item INNER JOIN shop_item ON shop_item.item_id=item.id WHERE shop_item.shop_id=:id";
    $items_id = array();
    try {
        $items_names = array();
        $db = getConnection();
        $stmt = $db->prepare($sql);         
        $stmt->bindParam("id", $id);        
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $items_names[] = array('item_name' => $row['item_name']);
        }       
        $array = array("items_names" => $items_names);
        $db = null;
        echo json_encode($array);  
    } catch(PDOException $e) {
        $data->id = -1;
        echo json_encode($data);
    }
}

// Actualiza las caracteristicas del jugador
function updateAttributes($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "UPDATE player SET strength=:strength, dextery=:dextery, intelligence=:intelligence, base_damage=:base_damage, max_health=:max_health, current_health=:current_health, mana=:mana, current_mana=:current_mana, money=:money WHERE user_id=:user_id";
        try {
            $db = getConnection();           
            $stmt = $db->prepare($sql);           
            $stmt->bindParam("user_id", $user_id);         
            $stmt->bindParam("strength", intval($data->strength));
            $stmt->bindParam("dextery", intval($data->dextery));
            $stmt->bindParam("intelligence", intval($data->intelligence));
            $stmt->bindParam("base_damage", intval($data->base_damage));
            $stmt->bindParam("max_health", intval($data->max_health));
            $stmt->bindParam("current_health", intval($data->current_health));
            $stmt->bindParam("mana", intval($data->mana));
            $stmt->bindParam("current_mana", intval($data->current_mana));            
            $stmt->bindParam("money", intval($data->money));
            $stmt->execute();       
            $db = null;           
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Equipa los objetos del jugador
function equipItems($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "SELECT id FROM item WHERE item_name=:item_name";
        $sql2 = "UPDATE player SET weapon=:weapon, accessory=:accessory WHERE user_id=:user_id";
        try {
            $db = getConnection();
            // Busca los id del accesorio y el arma           
            $stmt = $db->prepare($sql);                   
            $stmt->bindParam("item_name", $data->weapon_name);                 
            $stmt->execute();  
            $weapon;
            while ($row = $stmt->fetch())
            {                       
                $weapon = $row['id'];            
            }
            
            $stmt = $db->prepare($sql);                   
            $stmt->bindParam("item_name", $data->accessory_name);                 
            $stmt->execute();  
            $accessory;
            while ($row = $stmt->fetch())
            {                       
                $accessory = $row['id'];            
            }
                   
            $stmt = $db->prepare($sql2);           
            $stmt->bindParam("user_id", $user_id);         
            $stmt->bindParam("weapon", $weapon);     
            $stmt->bindParam("accessory", $accessory);         
            $stmt->execute();       
            $db = null;           
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Pone un objeto en el inventario
function addItem($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {
        try {
            $sql = "SELECT id FROM item WHERE item_name=:item_name";
            $sql2 = "SELECT id FROM player WHERE user_id=:user_id";
            $sql3 = "INSERT INTO inventory (player_id, item_id) VALUES (:player_id, :item_id)";
            
            $player_id;
            $item_id;
            $db = getConnection();
            $stmt = $db->prepare($sql);                       
            $stmt->bindParam("item_name", $data->item_name);
            $stmt->execute(); 
            while ($row = $stmt->fetch())
            {  
                $item_id = $row['id'];              
            }

            $db = getConnection();
            $stmt = $db->prepare($sql2);                       
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute(); 
            while ($row = $stmt->fetch())
            {  
                $player_id = $row['id'];              
            }

            $db = getConnection();
            $stmt = $db->prepare($sql3);                       
            $stmt->bindParam("player_id", $player_id);
            $stmt->bindParam("item_id", $item_id);
            $stmt->execute(); 

            $db = null;
        }
        catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
        }
    }
}

// Gestiona la ganancia de experiencia y subida de nivel
function addExperience($new_experience, $player_id) {

    $sql = "SELECT class, experience, player_level, current_experience FROM player WHERE id=:player_id";
    $sql2 = "UPDATE player SET experience=:experience, player_level=:level, current_experience=:current_experience WHERE id=:player_id";

    try {        
        $level;
        $experience;
        $current_experience;
        $class;
        $db = getConnection();
        $stmt = $db->prepare($sql);                       
        $stmt->bindParam("player_id", $player_id);
        $stmt->execute(); 
        while ($row = $stmt->fetch())
        {             
            $level = $row['player_level'];
            $experience = $row['experience'];
            $current_experience = $row['current_experience'];
            $class = $row['class'];
        }


        $current_experience =  $current_experience + $new_experience;
        if($current_experience >= $experience) {
            // Sube de nivel
            $current_experience = $current_experience - $experience;
            $level = $level + 1;
            $experience = 100 * ($level + 1);

            // Sube estadisticas y consigue habilidades
            if($class == "wizard") {
                $sql3 = "UPDATE player SET strength=strength+5, dextery=dextery+7, intelligence=intelligence+10, base_damage=base_damage, max_health=max_health+10, current_health=max_health, mana=mana+30, current_mana=mana WHERE id=:player_id";
            }
            else if ($class == "warrior"){
                $sql3 = "UPDATE player SET strength=strength+10, dextery=dextery+7, intelligence=intelligence+5, base_damage=base_damage, max_health=max_health+30, current_health=max_health, mana=mana+10, current_mana=mana WHERE id=:player_id";
            }
            else {
                $sql3 = "UPDATE player SET strength=strength+7, dextery=dextery+10, intelligence=intelligence+7, base_damage=base_damage, max_health=max_health+20, current_health=max_health, mana=mana+20, current_mana=mana WHERE id=:player_id";
            }
            $db = getConnection();
            $stmt = $db->prepare($sql3);
            $stmt->bindParam("player_id", $player_id);             
            $stmt->execute();
        }

        $db = getConnection();
        $stmt = $db->prepare($sql2);
        $stmt->bindParam("player_id", $player_id);               
        $stmt->bindParam("experience", $experience);
        $stmt->bindParam("level", $level);
        $stmt->bindParam("current_experience", $current_experience);
        $stmt->execute(); 
        $db = null;           
    } catch(PDOException $e) {
        $data->id = -1;
        echo $e;
        echo json_encode($data);
    }

    
}

// Da una recompensa tras el combate
function getReward($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {
        $sql = "SELECT id, money, player_level FROM player WHERE user_id=:user_id";
        $sql2 = "UPDATE player SET money=:money WHERE id=:player_id";
        try {
            $player_id;
            $money;
            $level;
            $experience;
            $current_experience;
            $db = getConnection();
            $stmt = $db->prepare($sql);                       
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();           
            while ($row = $stmt->fetch())
            {  
                $player_id = $row['id'];
                $money = $row['money'];
                $level = $row['player_level'];               
            }

            $new_money = ($data->enemies * $level * rand(1, $level) * 5);
            $money = $money + $new_money;
            
            $db = getConnection();
            $stmt = $db->prepare($sql2);                       
            $stmt->bindParam("player_id", $player_id);
            $stmt->bindParam("money", $money);
            $stmt->execute(); 

            $experience = ($data->enemies * $level * 10);
            
            addExperience($experience, $player_id);

            $array = array("money" => $new_money, "experience" => $experience);            
            $db = null;          
            echo json_encode($array); 
        } catch(PDOException $e) {
            $data->id = -1;
            echo $e;
            echo json_encode($data);
        }

    }
}

// Se obtienen todos los datos del jugador
function getPlayer($request) {
    $email = $request->getAttribute('email');
    $sql = "SELECT id FROM user WHERE email=:email";
    $sql2 = "SELECT id, money, accessory, weapon, name, class, faction, player_level, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence, experience, current_experience FROM player WHERE user_id=:user_id";
    $sql3 = "SELECT item_name FROM item INNER JOIN inventory ON inventory.item_id=item.id WHERE inventory.player_id=:player_id";
    $sql4 = "SELECT skill_name FROM skill INNER JOIN player_skill ON player_skill.skill_id=skill.id WHERE player_skill.player_id=:player_id";
    $sql5 = "SELECT id, item_name FROM item WHERE id=:accessory_id OR id=:weapon_id";
    try {
        $user_id = 0;
        $name;
        $class;
        $faction;
        $player_level;
        $strength;
        $base_damage;
        $max_health;
        $current_health;
        $mana;
        $current_mana;
        $dextery;
        $intelligence;
        $experience;
        $current_experience;
        $player_id;
        $weapon_id;
        $accessory_id;
        $money;

        $db = getConnection();
        $stmt = $db->prepare($sql);        
        $stmt->bindParam("email", $email);
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
                $user_id = $row['id'];
        } 
        // Se obtienen los datos  
        $stmt = $db->prepare($sql2);        
        $stmt->bindParam("user_id", $user_id);
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $player_id = $row['id'];
            $name = $row['name'];
            $class = $row['class'];
            $faction = $row['faction'];
            $player_level = $row['player_level'];
            $strength = $row['strength'];
            $base_damage = $row['base_damage'];
            $max_health = $row['max_health'];
            $current_health = $row['current_health'];
            $current_mana = $row['current_mana'];
            $mana = $row['mana'];
            $dextery = $row['dextery'];
            $intelligence = $row['intelligence'];
            $experience = $row['experience'];
            $current_experience = $row['current_experience'];
            $accessory_id = $row['accessory'];
            $weapon_id = $row['weapon'];
            $money = $row['money'];
        }

        // Se obtienen los objetos del jugador
        $inventory = array();
        $stmt = $db->prepare($sql3);        
        $stmt->bindParam("player_id", $player_id);
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $inventory[] = array('item_name' => $row['item_name']);
        }  

        // Se obtienen las habilidades del jugador
        $skills = array();
        $stmt = $db->prepare($sql4);        
        $stmt->bindParam("player_id", $player_id);
        $stmt->execute();
        while ($row = $stmt->fetch())
        {  
            $skills[] = array('skill_name' => $row['skill_name']);
        }

        $weapon;
        $accessory;
        // Se obtienen el arma y el accesorio
        $stmt = $db->prepare($sql5);        
        $stmt->bindParam("accessory_id", $accessory_id);
        $stmt->bindParam("weapon_id", $weapon_id);
        $stmt->execute();
        $weapon = "none";
        $accessory = "none";
        while ($row = $stmt->fetch())
        {  
            if($row['id'] == $accessory_id) {
                $accessory = $row['item_name'];
            }
            else if($row['id'] == $weapon_id) {
                $weapon = $row['item_name'];
            }
        }

        $array = array("weapon" => $weapon, "accessory" => $accessory, "name" => $name, "class" => $class, "faction" => $faction, "player_level" => $player_level, "strength" => $strength, "base_damage" => $base_damage, "max_health" => $max_health, "current_health" => $current_health, "mana" => $mana, "current_mana" => $current_mana, "dextery" => $dextery, "intelligence" => $intelligence, "experience" => $experience, "current_experience" => $current_experience, "money" => $money, "inventory" => $inventory, "skills" => $skills);
        $db = null;
        echo json_encode($array);
        } catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
    }
}

// Eliminar un objeto del inventario
function deleteItem($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "SELECT id FROM player WHERE user_id=:user_id";
        $sql2 = "SELECT id FROM item WHERE item_name=:item_name";
        $sql3 = "DELETE FROM inventory WHERE player_id=:player_id AND item_id=:item_id LIMIT 1";      
        try {          
            $db = getConnection();            

            // Se obtiene la id del jugador
            $stmt = $db->prepare($sql);        
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $player_id;
            while ($row = $stmt->fetch())
            {  
                $player_id = $row['id'];           
            }

            // Se obtiene la id del jugador
            $item_id;
            $stmt = $db->prepare($sql2);        
            $stmt->bindParam("item_name", $data->item_name);
            $stmt->execute();          
            while ($row = $stmt->fetch())
            {  
                $item_id = $row['id'];           
            }

            // Se borra el objeto del inventario
            $stmt = $db->prepare($sql3);        
            $stmt->bindParam("player_id", $player_id);
            $stmt->bindParam("item_id", $item_id);
            $stmt->execute();
            $db = null;
            echo json_encode($data);  
        } catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
        }
    }
}

// Crear el jugador
function createPlayer($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);
            
    if($user_id != 0) {
        $sql = "INSERT INTO player (name, class, faction, player_level, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence, experience, current_experience, user_id) VALUES (:name, :class, :faction, :player_level, :strength, :base_damage, :max_health, :current_health, :mana, :current_mana, :dextery, :intelligence, :experience, :current_experience, :user_id)";
        $sql2 = "SELECT id FROM player WHERE name=:name";
        $sql3 = "INSERT INTO player_skill (player_id, skill_id) VALUES (:player_id, :skill_id)";
        try {
            $player_level = 1;
            $base_damage = 1;
            $experience = 100;
            $current_experience = 0;
            $strength = 0;
            $intelligence = 0;
            $dextery = 0;
            $mana = 0;;
            $health = 0;
            $skill = 0;
            $db = getConnection();
            $stmt = $db->prepare($sql);
            $stmt->bindParam("name", $data->name);
            $stmt->bindParam("class", $data->class);
            $stmt->bindParam("faction", $data->faction);
            $stmt->bindParam("player_level", $player_level);
            $stmt->bindParam("base_damage", $base_damage);
            $stmt->bindParam("experience", $experience);
            $stmt->bindParam("current_experience", $current_experience);
            $stmt->bindParam("user_id", $user_id);
            if($data->class == "warrior") {
                $strength = 12;
                $intelligence = 8;
                $dextery = 10;
                $mana = 40;;
                $health = 100; 
                $skill = 1;               
            }
            else if($data->class == "wizard") {
                $strength = 8;
                $intelligence = 12;
                $dextery = 10;
                $mana = 100;;
                $health = 70;   
                $skill = 1;          
            }
            else if($data->class == "swordsman") {
                $strength = 10;
                $intelligence = 10;
                $dextery = 12;
                $mana = 80;
                $health = 85;
                $skill = 1;
            }
            $stmt->bindParam("strength", $strength);
            $stmt->bindParam("max_health", $health);
            $stmt->bindParam("current_health", $health);
            $stmt->bindParam("mana", $mana);
            $stmt->bindParam("current_mana", $mana);
            $stmt->bindParam("dextery", $dextery);
            $stmt->bindParam("intelligence", $intelligence);
            $stmt->execute();
            // Ponemos las habilidades
            $stmt = $db->prepare($sql2);
            $stmt->bindParam("name", $data->name);
            $stmt->execute();
            // Obtenemos la id del  jugador para poner sus habilidades
            while ($row = $stmt->fetch())
            {  
                $player_id = $row['id'];
            }      
            // Ponemos la habilidad
            $stmt = $db->prepare($sql3);
            $stmt->bindParam("player_id", $player_id);
            $stmt->bindParam("skill_id", $skill);
            $stmt->execute();
            // Devolvemos la id adquirida y los datos enviados
            $data->id = $db->lastInsertId();
            $db = null;
            echo json_encode($data);
        } catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
        }
    } 

}

// Obtiene las misiones
function getQuests($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "SELECT quest_id, progress FROM accepted_quest WHERE user_id=:user_id";      
        $sql2 = "SELECT quest_id FROM delivered_quest WHERE user_id=:user_id";
        $sql3 = "SELECT id, title, description, number, experience, money FROM quest";

        try {
            $db = getConnection();     
            // Obtenemos las misiones en progreso      
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $accepted_quests_id = array();
            $quests_progress = array();
            while ($row = $stmt->fetch())
            {  
                $accepted_quests_id[] = $row['quest_id'];
                $quests_progress[] = $row['progress'];
            }

            // Obtenemos las misiones entregadas que no deberan mostrarse
            $stmt = $db->prepare($sql2); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $delivered_quests_id = array();
            while ($row = $stmt->fetch())
            {  
                $delivered_quests_id[] = $row['quest_id'];
            }    
            

            $stmt = $db->prepare($sql3);          
            $stmt->execute();
            /*$quests_id = array();
            $quests_titles = array();
            $quests_descriptions = array();
            $quests_numbers = array();
            $quests_experience = array();
            $quests_money = array();
            $all_quests_progress = array();*/
            $quests = array();
            while ($row = $stmt->fetch())
            {  
                $canShowQuest = true;
                $progress = "-1";
                for($i = 0; $i < count($delivered_quests_id); $i++) {
                    if($delivered_quests_id[$i] == $row['id']) {
                        $canShowQuest = false;
                    }
                }
                for($i = 0; $i < count($accepted_quests_id); $i++) {
                    if($accepted_quests_id[$i] == $row['id']) {
                        $progress = $quests_progress[$i];
                    }
                }
                if($canShowQuest) {
                    $quests[] = array("id" => $row['id'], "title" => $row['title'], "description" => $row['description'], "number" => $row['number'], "experience" => $row['experience'], "money" => $row['money'], "progress" => $progress);
                    /*$quests_id[] = array("id" => $row['id']);
                    $quests_titles[] = array("title" => $row['title']);
                    $quests_descriptions[] = array("description" => $row['description']);
                    $quests_numbers[] = array("number" => $row['number']);
                    $quests_experience[] = array("experience" => $row['experience']);
                    $quests_money[] = array("money" => $row['money']);
                    $all_quests_progress[] = array("progress" => $progress);  */                  
                }
            }     
            //$array = array("quests_id" => $quests_id, "quests_titles" => $quests_titles, "quests_descriptions" => $quests_descriptions, "numbers" => $quests_numbers, "quests_experience" => $quests_experience, "quests_money" => $quests_money, "quests_progress" => $all_quests_progress);        
            $array = array("quests" => $quests);
            $db = null;    
            echo json_encode($array, JSON_UNESCAPED_UNICODE);
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}



// Obtiene las misiones en progreso
function getAcceptedQuests($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "SELECT quest_id, progress FROM accepted_quest WHERE user_id=:user_id";       
        $sql2 = "SELECT id, title, description, number, experience, money, enemy_id FROM quest";
        $sql3 = "SELECT name FROM enemy WHERE id=:enemy_id";

        try {
            $db = getConnection();     
            // Obtenemos las misiones en progreso      
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $accepted_quests_id = array();
            $quests_progress = array();
            while ($row = $stmt->fetch())
            {  
                $accepted_quests_id[] = $row['quest_id'];
                $quests_progress[] = $row['progress'];
            }

            $stmt = $db->prepare($sql2);          
            $stmt->execute();
            /*$quests_id = array();
            $quests_titles = array();
            $quests_descriptions = array();
            $quests_numbers = array();
            $quests_experience = array();
            $quests_money = array();
            $all_quests_progress = array();*/
            $quests = array();
            $progress;
            while ($row = $stmt->fetch())
            {  
                $canShowQuest = false;
                $progress = "0";                
                for($i = 0; $i < count($accepted_quests_id); $i++) {
                    if($accepted_quests_id[$i] == $row['id']) {
                        $progress = $quests_progress[$i];
                        $canShowQuest = true;
                    }
                }
                if($canShowQuest) {
                    $enemy_id = $row['enemy_id'];
                    $stmt2 = $db->prepare($sql3); 
                    $stmt2->bindParam("enemy_id", $enemy_id);
                    $stmt2->execute();                  
                    while ($row2 = $stmt2->fetch())
                    {  
                        $enemy_name = $row2['name'];
                    }
                    $quests[] = array("id" => $row['id'], "title" => $row['title'], "description" => $row['description'], "number" => $row['number'], "experience" => $row['experience'], "money" => $row['money'], "progress" => $progress, "enemy_name" => $enemy_name);
                    
                    /*$quests_id[] = array("id" => $row['id']);
                    $quests_titles[] = array("title" => $row['title']);
                    $quests_descriptions[] = array("description" => $row['description']);
                    $quests_numbers[] = array("number" => $row['number']);
                    $quests_experience[] = array("experience" => $row['experience']);
                    $quests_money[] = array("money" => $row['money']);
                    $all_quests_progress[] = array("progress" => $progress);  */                  
                }
            }     
            //$array = array("quests_id" => $quests_id, "quests_titles" => $quests_titles, "quests_descriptions" => $quests_descriptions, "numbers" => $quests_numbers, "quests_experience" => $quests_experience, "quests_money" => $quests_money, "quests_progress" => $all_quests_progress);        
            $array = array("quests" => $quests);
            $db = null;    
            echo json_encode($array, JSON_UNESCAPED_UNICODE);
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Acepta una mision
function progressInQuest($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "UPDATE accepted_quest SET progress=progress+:progress WHERE quest_id=:quest_id AND user_id=:user_id"; 
        
        try {
            $db = getConnection();     
            // Obtenemos las misiones en progreso      
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->bindParam("quest_id", $data->id);
            $stmt->bindParam("progress", $data->progress);
            $stmt->execute();          
            $db = null;
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Acepta una mision
function acceptQuest($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "INSERT INTO accepted_quest (user_id, quest_id) VALUES (:user_id, :quest_id)"; 
        
        try {
            $db = getConnection();     
            // Obtenemos las misiones en progreso      
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->bindParam("quest_id", $data->id);
            $stmt->execute();          
            $db = null;
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}


// Completa una mision
function completeQuest($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "INSERT INTO delivered_quest (user_id, quest_id) VALUES (:user_id, :quest_id)";
        $sql2 = "SELECT experience, money FROM quest WHERE id=:quest_id";
        $sql3 = "UPDATE player SET money=:money WHERE user_id=:user_id";
        $sql4 = "SELECT id FROM player WHERE user_id=:user_id";
        $sql5 = "DELETE FROM accepted_quest WHERE user_id=:user_id AND quest_id=:quest_id LIMIT 1";
        
        try {
            $db = getConnection();     
            // Obtenemos las misiones en progreso      
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->bindParam("quest_id", $data->id);
            $stmt->execute();   
            
            $stmt = $db->prepare($sql2);
            $stmt->bindParam("quest_id", $data->id);
            $stmt->execute();
            $experience;
            $money;
            while ($row = $stmt->fetch())
            {  
                $experience = $row['experience'];
                $money = $row['money'];
            }

            $stmt = $db->prepare($sql3);
            $stmt->bindParam("money", $money);
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();

            $player_id;
            $stmt = $db->prepare($sql4);
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();            
            while ($row = $stmt->fetch())
            {  
                $player_id = $row['id'];
            }           
            addExperience($experience, $player_id);

            $stmt = $db->prepare($sql5); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->bindParam("quest_id", $data->id);
            $stmt->execute();   

            $db = null;
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}



// Usa los puntos de valentia y devuelve el jefe
function useBraveryPoints($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "UPDATE user SET bravery_points=bravery_points-:points WHERE id=:user_id"; 
        $sql2 = "SELECT name FROM enemy WHERE id=:boss_id"; 

        try {
            $db = getConnection();              
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->bindParam("points", $data->bravery_points);
            $stmt->execute();         
            
            $boss_name;
            $stmt = $db->prepare($sql2); 
            $stmt->bindParam("boss_id", $data->boss_id);
            $stmt->execute(); 
            while ($row = $stmt->fetch())
            {  
                $boss_name = $row['name'];
            }

            $array = array("boss_name" => $boss_name);
            $db = null;    
            echo json_encode($array, JSON_UNESCAPED_UNICODE);
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Se obtiene un punto de valentia
function addBraveryPoint($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "UPDATE user SET bravery_points=bravery_points+1 WHERE id=:user_id"; 

        try {
            $db = getConnection();              
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();

            $db = null;          
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

function addFactionPoints($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        
        $sql = "SELECT player_level, faction FROM player WHERE user_id=:user_id"; 

        try {
            $db = getConnection();              
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();

            while ($row = $stmt->fetch())
            {  
                $faction = $row['faction'];
                $level = $row['player_level'];
            }

            $sql2 = "UPDATE faction SET points=points+:level WHERE name=:name";
            $name;
            if($faction == "engineering") {
                $name = "Engineering";
            }
            else if($faction == "science") {
                $name = "Science";
            }
            else {
                $name = "ArtsAndHumanities";
            }

            $db = getConnection();              
            $stmt = $db->prepare($sql2); 
            $stmt->bindParam("name", $name);
            $stmt->bindParam("level", $level);
            $stmt->execute();

            $db = null;          
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

// Se inicia la batalla online
function startOnlineBattle($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {  
        $sql = "SELECT id, current_health FROM player WHERE user_id=:user_id";
        $sql2 = "SELECT id, name, class, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence FROM player WHERE id=:second_player_id";
        $sql3 = "INSERT INTO player_battle (first_player, second_player, first_player_command, second_player_command, first_player_damage, second_player_damage, first_player_health, second_player_health) VALUES (:first_player, :second_player, :first_player_command, :second_player_command, :first_player_damage, :second_player_damage, :first_player_health, :second_player_health)"; 
        $sql4 = "INSERT INTO player_battle_stats (player_id, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence, command, finish_action) VALUES (:player_id, :strength, :base_damage, :max_health, :current_health, :mana, :current_mana, :dextery, :intelligence, :command, :finish_action)";

        try {
            $db = getConnection(); 

            // Se obtienen los datos del primer jugador        
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $id;
            $current_health;
            while ($row = $stmt->fetch())
            {                 
                $current_health = $row['current_health'];
                $id = $row['id'];
            }

            // Se obtienen los datos del segundo jugador
            $stmt = $db->prepare($sql2); 
            $stmt->bindParam("second_player_id", $data->second_player_id);
            $stmt->execute();
            $name;
            $class;                
            $strength;
            $base_damage;
            $max_health;
            $second_current_health;
            $mana;
            $current_mana;
            $dextery;
            $intelligence;
            $online_player_id;       
            while ($row = $stmt->fetch())
            {                
                $name = $row['name'];
                $class = $row['class'];                       
                $strength = $row['strength'];
                $base_damage = $row['base_damage'];
                $max_health = $row['max_health'];
                $second_current_health = $row['current_health'];
                $current_mana = $row['current_mana'];
                $mana = $row['mana'];
                $dextery = $row['dextery'];
                $intelligence = $row['intelligence'];
                $online_player_id = $row['id'];
            }


            // Se crea la tabla de la batalla    
            $db = getConnection();              
            $stmt = $db->prepare($sql3); 
            $stmt->bindParam("first_player", $id);
            $stmt->bindParam("second_player", $online_player_id);
            $command = "none";
            $stmt->bindParam("first_player_command", $command);
            $stmt->bindParam("second_player_command", $command);
            $damage = 0;
            $stmt->bindParam("first_player_damage", $damage);
            $stmt->bindParam("second_player_damage", $damage);            
            $stmt->bindParam("first_player_health", $current_health);
            $stmt->bindParam("second_player_health", $second_current_health);
            $stmt->execute();
            $last_id = $db->lastInsertId();

            // Se crea la tabla de los stats del segundo jugador
            $stmt = $db->prepare($sql4); 
            $stmt->bindParam("strength", $strength);
            $stmt->bindParam("base_damage", $base_damage);
            $stmt->bindParam("max_health", $max_health);
            $stmt->bindParam("current_health", $second_current_health);
            $stmt->bindParam("mana", $mana);
            $stmt->bindParam("current_mana", $current_mana);
            $stmt->bindParam("dextery", $dextery);
            $stmt->bindParam("intelligence", $intelligence);
            $stmt->bindParam("player_id", $online_player_id);
            $command = "none";
            $finish_action = "no";
            $stmt->bindParam("command", $command);
            $stmt->bindParam("finish_action", $finish_action);
            $stmt->execute();
        
            // Se devuelven los datos
            $array = array("online_player_id" => $online_player_id, "online_battle_id" => $last_id, "local_player_id" => $id, "name" => $name, "class" => $class, "strength" => $strength, "base_damage" => $base_damage, "max_health" => $max_health, "current_health" => $second_current_health, "mana" => $mana, "current_mana" => $current_mana, "dextery" => $dextery, "intelligence" => $intelligence);

            $db = null;  
            echo json_encode($array);      
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

function checkNotifications($request) {
    $data = json_decode($request->getBody());
 
    // Comprobamos que los datos son correctos
    $user_id = getUserId($data->email, $data->user_password);

    if($user_id != 0) {        
        $sql = "SELECT id FROM player WHERE user_id=:user_id";
        $sql1 = "SELECT id, first_player FROM player_battle WHERE second_player=:second_player_id";
        $sql2 = "SELECT name, class, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence FROM player WHERE id=:first_player_id";       
        $sql3 = "INSERT INTO player_battle_stats (player_id, strength, base_damage, max_health, current_health, mana, current_mana, dextery, intelligence, command, finish_action) VALUES (:player_id, :strength, :base_damage, :max_health, :current_health, :mana, :current_mana, :dextery, :intelligence, :command, :finish_action)";

        try {
            $db = getConnection(); 

            // Se obtienen los datos del segundo jugador    
            $stmt = $db->prepare($sql); 
            $stmt->bindParam("user_id", $user_id);
            $stmt->execute();
            $id;
            while ($row = $stmt->fetch())
            {                                
                $id = $row['id'];
            }

            $last_id;
            $online_player_id;
            // Se comprueba si se ha iniciado un combate    
            $stmt = $db->prepare($sql1); 
            $stmt->bindParam("second_player_id", $id);
            $stmt->execute();
            $id;
            $exist = false;
            while ($row = $stmt->fetch())
            {                                
                $last_id = $row['id'];
                $online_player_id = $row['first_player'];
                $exist = true;          
            }

            if($exist) {
                // Se obtienen los datos del primer jugador
                $stmt = $db->prepare($sql2); 
                $stmt->bindParam("first_player_id", $online_player_id);
                $stmt->execute();
                $name;
                $class;                
                $strength;
                $base_damage;
                $max_health;
                $second_current_health;
                $mana;
                $current_mana;
                $dextery;
                $intelligence;          
                while ($row = $stmt->fetch())
                {                
                    $name = $row['name'];
                    $class = $row['class'];                       
                    $strength = $row['strength'];
                    $base_damage = $row['base_damage'];
                    $max_health = $row['max_health'];
                    $second_current_health = $row['current_health'];
                    $current_mana = $row['current_mana'];
                    $mana = $row['mana'];
                    $dextery = $row['dextery'];
                    $intelligence = $row['intelligence'];   
                }
                
                // Se crea la tabla de los stats del primer jugador
                $stmt = $db->prepare($sql3); 
                $stmt->bindParam("strength", $strength);
                $stmt->bindParam("base_damage", $base_damage);
                $stmt->bindParam("max_health", $max_health);
                $stmt->bindParam("current_health", $second_current_health);
                $stmt->bindParam("mana", $mana);
                $stmt->bindParam("current_mana", $current_mana);
                $stmt->bindParam("dextery", $dextery);
                $stmt->bindParam("intelligence", $intelligence);
                $stmt->bindParam("player_id", $online_player_id);
                $command = "none";
                $finish_action = "no";
                $stmt->bindParam("command", $command);
                $stmt->bindParam("finish_action", $finish_action);
                $stmt->execute();

                // Se devuelven los datos
                $online_battle_start = "yes";
                $array = array("online_battle" => $online_battle_start, "online_player_id" => $online_player_id, "online_battle_id" => $last_id, "local_player_id" => $id, "name" => $name, "class" => $class, "strength" => $strength, "base_damage" => $base_damage, "max_health" => $max_health, "current_health" => $second_current_health, "mana" => $mana, "current_mana" => $current_mana, "dextery" => $dextery, "intelligence" => $intelligence);
            }

            else {
                $online_battle_start = "no";
                $array = array("online_battle" => $online_battle_start);
            }            

            $db = null;  
            echo json_encode($array);      
        } catch(PDOException $e) {
            echo $e;
            $data->id = -1;
            echo json_encode($data);
        }

    }
}

function setPlayerBattleStats($request) {
    $data = json_decode($request->getBody());

    $sql = "UPDATE player_battle_stats SET strength=:strength, base_damage=:base_damage, current_health=:current_health, current_mana=:current_mana, dextery=:dextery, intelligence=:intelligence WHERE player_id=:player_id";
       
    try {

        $db = getConnection();

        $stmt = $db->prepare($sql); 
        $stmt->bindParam("player_id", $data->player_id);
        $stmt->bindParam("strength", $data->strength);
        $stmt->bindParam("base_damage", $data->base_damage);
        $stmt->bindParam("current_health", $data->current_health);
        $stmt->bindParam("current_mana", $data->current_mana);
        $stmt->bindParam("dextery", $data->dextery);
        $stmt->bindParam("intelligence", $data->intelligence);
        $stmt->execute();

        $db = null;
        echo json_encode($data);
    } catch(PDOException $e) {
        echo $e;
        $data->id = -1;
        echo json_encode($data);
    }

}

function getPlayerBattleStats($request) {
    $data = json_decode($request->getBody());
 
    $sql = "SELECT strength, base_damage, current_health, current_mana, dextery, intelligence FROM player_battle_stats WHERE player_id=:player_id";
    
    try {

        $db = getConnection(); 
                   
        $stmt = $db->prepare($sql); 
        $stmt->bindParam("player_id", $data->player_id);
        $stmt->execute();
        $strength;
        $base_damage;
        $current_health;
        $current_mana;
        $dextery;
        $intelligence;          
        while ($row = $stmt->fetch())
        {                                   
            $strength = $row['strength'];
            $base_damage = $row['base_damage'];        
            $current_health = $row['current_health'];
            $current_mana = $row['current_mana'];
            $dextery = $row['dextery'];
            $intelligence = $row['intelligence'];   
        }    

        $array = array("strength" => $strength, "base_damage" => $base_damage, "current_health" => $current_health, "current_mana" => $current_mana, "dextery" => $dextery, "intelligence" => $intelligence);
        $db = null;
        echo json_encode($array);
        
    } catch(PDOException $e) {
        echo $e;
        $data->id = -1;
        echo json_encode($data);
    }

}

function getOnlineBattleCommand($request) {
    $data = json_decode($request->getBody());

    $sql = "SELECT command, finish_action FROM player_battle_stats WHERE player_id=:player_id";
        
    try {
        $db = getConnection();                    
        $stmt = $db->prepare($sql); 
        $stmt->bindParam("player_id", $data->player_id);
        $stmt->execute();
        $command;
        $finish_action;
        while ($row = $stmt->fetch())
        {                
            $command = $row['command'];
            $finish_action = $row['finish_action'];              
        }

        $db = null;
        $array = array("command" => $command, "finish_action" => $finish_action);
        echo json_encode($array);

    } catch(PDOException $e) {
        echo $e;
        $data->id = -1;
        echo json_encode($data);
    }

}

function setOnlineBattleCommand($request) {
    $data = json_decode($request->getBody());

    $sql = "UPDATE player_battle_stats SET command=:command, finish_action=:finish_action WHERE player_id=:player_id";
        
    try {
        $db = getConnection(); 

        $stmt = $db->prepare($sql); 
        $stmt->bindParam("player_id", $data->player_id);
        $stmt->bindParam("finish_action", $data->finish_action);
        $stmt->bindParam("command", $data->command);
        $stmt->execute();

        $db = null;    
        echo json_encode($data);
    } catch(PDOException $e) {
        echo $e;
        $data->id = -1;
        echo json_encode($data);
    }
}

function getUser($request) {
    $data = json_decode($request->getBody());
    
    // Comprobamos si existe
    $exits = userAlreadyExits($data->user_login);

    if(!($exits) && !($email_exits)) {
        $salt = md5($data->user_password);
        $encrypted_password = crypt($data->user_password, $salt);

        $sql = "SELECT * FROM user WHERE name=:user_name";
        try {
            $db = getConnection();
            $stmt = $db->prepare($sql);
            $stmt->bindParam("user_login", $data->user_login);
            $stmt->bindParam("user_password", $encrypted_password);
            $stmt->bindParam("email", $data->email);
            $stmt->execute();
            // Devolvemos la id adquirida y los datos enviados
            $data->id = $db->lastInsertId();
            $db = null;
            echo json_encode($data);
        } catch(PDOException $e) {
            $data->id = -1;
            echo json_encode($data);
        }
    }

    else {
        $data->id = 0;
        if($exits) {
            $data->user_login = "exits";
        }
        if($email_exits) {
            $data->email = "exits";
        }
        echo json_encode($data);
    }
}