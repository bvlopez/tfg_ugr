-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 18-06-2019 a las 17:30:27
-- Versión del servidor: 10.1.38-MariaDB
-- Versión de PHP: 7.3.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `gamedb`
--
CREATE DATABASE IF NOT EXISTS `gamedb` DEFAULT CHARACTER SET utf8 COLLATE utf8_spanish_ci;
USE `gamedb`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `accepted_quest`
--

CREATE TABLE `accepted_quest` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `quest_id` int(11) NOT NULL,
  `progress` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `accepted_quest`
--

INSERT INTO `accepted_quest` (`id`, `user_id`, `quest_id`, `progress`) VALUES
(5, 87, 1, 0),
(6, 87, 2, 0),
(7, 87, 4, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `boss_enemy`
--

CREATE TABLE `boss_enemy` (
  `id` int(11) NOT NULL,
  `enemy_id` int(11) NOT NULL,
  `latitude` decimal(15,13) NOT NULL,
  `longitude` decimal(14,13) NOT NULL,
  `bravery_points_required` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `boss_enemy`
--

INSERT INTO `boss_enemy` (`id`, `enemy_id`, `latitude`, `longitude`, `bravery_points_required`) VALUES
(1, 9, '37.1988730000000', '-3.6243960000000', 10),
(2, 10, '37.1963370000000', '-3.6260670000000', 5),
(3, 11, '37.1983140000000', '-3.6266480000000', 2),
(4, 10, '37.1864770000000', '-3.6010180000000', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `chat_line`
--

CREATE TABLE `chat_line` (
  `id` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `second_player_id` int(11) NOT NULL,
  `text` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `chat_line`
--

INSERT INTO `chat_line` (`id`, `owner`, `second_player_id`, `text`, `date`) VALUES
(16, 13, 6, 'hola', '2019-06-12 00:14:02'),
(17, 6, 13, 'hola', '2019-06-12 00:14:22'),
(18, 13, 6, 'adios', '2019-06-12 00:14:33'),
(19, 6, 13, 'adios', '2019-06-12 00:14:39');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `delivered_quest`
--

CREATE TABLE `delivered_quest` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `quest_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `delivered_quest`
--

INSERT INTO `delivered_quest` (`id`, `user_id`, `quest_id`) VALUES
(8, 87, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `enemy`
--

CREATE TABLE `enemy` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `is_boss` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `enemy`
--

INSERT INTO `enemy` (`id`, `name`, `is_boss`) VALUES
(1, 'Araña', 0),
(2, 'Escorpión', 0),
(3, 'Murciélago', 0),
(4, 'Rata', 0),
(5, 'Slime', 0),
(6, 'Esqueleto', 0),
(7, 'Serpiente', 0),
(8, 'Hombre Lagarto', 0),
(9, 'Aurum', 1),
(10, 'Goliath', 1),
(11, 'Daemarbora', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `faction`
--

CREATE TABLE `faction` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `points` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `faction`
--

INSERT INTO `faction` (`id`, `name`, `points`) VALUES
(1, 'Science', 26),
(2, 'Engineering', 10),
(3, 'ArtsAndHumanities', 8);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `guild`
--

CREATE TABLE `guild` (
  `id` int(11) NOT NULL,
  `latitude` decimal(15,13) NOT NULL,
  `longitude` decimal(14,13) NOT NULL,
  `name` varchar(255) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `guild`
--

INSERT INTO `guild` (`id`, `latitude`, `longitude`, `name`) VALUES
(1, '37.1972670000000', '-3.6245570000000', 'Campus de Aynadamar');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inventory`
--

CREATE TABLE `inventory` (
  `id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inventory`
--

INSERT INTO `inventory` (`id`, `player_id`, `item_id`) VALUES
(25, 6, 1),
(28, 6, 2),
(34, 6, 2),
(38, 6, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `item`
--

CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `item_name` varchar(255) COLLATE utf8_spanish_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `item`
--

INSERT INTO `item` (`id`, `item_name`) VALUES
(1, 'Espada rota'),
(2, 'Anillo de fuerza'),
(3, 'Pocion de curacion debil'),
(4, 'Anillo del sabio'),
(5, 'Anillo del combatiente'),
(6, 'Sable'),
(7, 'Maza'),
(8, 'Piedra de recuperación debil');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `player`
--

CREATE TABLE `player` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `class` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `faction` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `player_level` int(11) DEFAULT NULL,
  `strength` int(11) DEFAULT NULL,
  `base_damage` int(11) DEFAULT NULL,
  `max_health` int(11) DEFAULT NULL,
  `current_health` int(11) DEFAULT NULL,
  `mana` int(11) DEFAULT NULL,
  `current_mana` int(11) DEFAULT NULL,
  `dextery` int(11) DEFAULT NULL,
  `intelligence` int(11) DEFAULT NULL,
  `experience` int(11) DEFAULT NULL,
  `current_experience` int(11) DEFAULT NULL,
  `accessory` int(11) DEFAULT NULL,
  `weapon` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `money` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `player`
--

INSERT INTO `player` (`id`, `name`, `class`, `faction`, `player_level`, `strength`, `base_damage`, `max_health`, `current_health`, `mana`, `current_mana`, `dextery`, `intelligence`, `experience`, `current_experience`, `accessory`, `weapon`, `user_id`, `money`) VALUES
(6, 'prueba2', 'wizard', 'science', 4, 10, 1, 100, 100, 230, 230, 107, 110, 500, 215, 2, 1, 87, 160),
(9, 'prueba3', 'warrior', 'engineering', 1, 12, 1, 100, 100, 40, 40, 10, 8, 100, 100, NULL, NULL, 92, 0),
(10, 'prueba4', 'wizard', 'science', 1, 8, 1, 70, 70, 100, 100, 10, 12, 100, 100, NULL, NULL, 93, 0),
(11, 'prueba5', 'wizard', 'science', 1, 8, 1, 70, 70, 100, 100, 10, 12, 100, 100, NULL, NULL, 94, 0),
(12, 'prueba6', 'swordsman', 'engineering', 1, 12, 1, 100, 8, 40, 40, 10, 8, 100, 100, NULL, 1, 95, 0),
(13, 'prueba7', 'warrior', 'science', 2, 22, 1, 130, 130, 50, 50, 17, 13, 300, 30, 2, 1, 96, 15),
(14, 'prueba17', 'swordsman', 'artsandhumanities', 1, 10, 1, 85, 85, 80, 80, 12, 10, 100, 0, NULL, NULL, 97, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `player_battle`
--

CREATE TABLE `player_battle` (
  `id` int(11) NOT NULL,
  `first_player` int(11) NOT NULL,
  `second_player` int(11) NOT NULL,
  `first_player_command` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `second_player_command` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `first_player_damage` int(11) NOT NULL,
  `second_player_damage` int(11) NOT NULL,
  `first_player_health` int(11) NOT NULL,
  `second_player_health` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `player_battle_stats`
--

CREATE TABLE `player_battle_stats` (
  `id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `strength` int(11) NOT NULL,
  `base_damage` int(11) NOT NULL,
  `max_health` int(11) NOT NULL,
  `current_health` int(11) NOT NULL,
  `mana` int(11) NOT NULL,
  `current_mana` int(11) NOT NULL,
  `dextery` int(11) NOT NULL,
  `intelligence` int(11) NOT NULL,
  `command` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `finish_action` varchar(255) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `player_skill`
--

CREATE TABLE `player_skill` (
  `id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `skill_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `player_skill`
--

INSERT INTO `player_skill` (`id`, `player_id`, `skill_id`) VALUES
(1, 6, 1),
(4, 9, 1),
(5, 10, 1),
(6, 11, 1),
(7, 12, 1),
(8, 13, 1),
(9, 14, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `quest`
--

CREATE TABLE `quest` (
  `id` int(11) NOT NULL,
  `title` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `description` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `enemy_id` int(11) NOT NULL,
  `number` int(11) NOT NULL,
  `experience` int(11) NOT NULL,
  `money` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `quest`
--

INSERT INTO `quest` (`id`, `title`, `description`, `enemy_id`, `number`, `experience`, `money`) VALUES
(1, 'Derrotar a 5 arañas', 'Han aparecido un gran número de arañas en la ciudad, ayuda al gremio', 1, 5, 10, 10),
(2, 'Derrotar 20 Arañas', 'El número de arañas cada vez es mayor, tienes que ayudar al gremio', 1, 20, 50, 50),
(3, 'Derrotar 100 Arañas', 'Acabemos con las arañas de una vez por todas', 1, 100, 100, 100),
(4, 'Derrotar a los Goliath', 'Derrota a los escorpiones gigantes', 10, 3, 50, 50);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `shop`
--

CREATE TABLE `shop` (
  `id` int(11) NOT NULL,
  `latitude` decimal(15,13) NOT NULL,
  `longitude` decimal(14,13) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `shop`
--

INSERT INTO `shop` (`id`, `latitude`, `longitude`) VALUES
(2, '37.1977040000000', '-3.6261470000000'),
(3, '37.1968490000000', '-3.6230710000000'),
(4, '37.1860630000000', '-3.6013180000000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `shop_item`
--

CREATE TABLE `shop_item` (
  `id` int(11) NOT NULL,
  `shop_id` int(11) DEFAULT NULL,
  `item_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `shop_item`
--

INSERT INTO `shop_item` (`id`, `shop_id`, `item_id`) VALUES
(1, 2, 1),
(2, 2, 2),
(3, 2, 3),
(4, 3, 2),
(5, 4, 2),
(6, 4, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `skill`
--

CREATE TABLE `skill` (
  `id` int(11) NOT NULL,
  `skill_name` varchar(255) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `skill`
--

INSERT INTO `skill` (`id`, `skill_name`) VALUES
(1, 'Bola de fuego'),
(2, 'Aumento'),
(3, 'Miedo'),
(4, 'Tormenta de fuego'),
(5, 'Gran aumento'),
(6, 'Supresion');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `email` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `user_password` varchar(255) COLLATE utf8_spanish_ci NOT NULL,
  `latitude` decimal(15,13) DEFAULT NULL,
  `longitude` decimal(14,13) DEFAULT NULL,
  `last_connection` datetime DEFAULT NULL,
  `bravery_points` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `user`
--

INSERT INTO `user` (`id`, `email`, `user_password`, `latitude`, `longitude`, `last_connection`, `bravery_points`) VALUES
(53, 'paco', 'f6m8/57NyPefs', '37.1978410000000', '-3.6238480000000', '2019-04-21 20:16:32', 0),
(54, 'asdasdasdasrg', 'd5qjWhBbmrFzU', '37.1978410000000', '-3.6238480000000', '2019-04-10 18:01:05', 0),
(55, 'asdasd', '78vQ2aH.4eCqc', '37.1978271724370', '-3.6253539802018', '2019-04-10 18:22:26', 0),
(57, 'asd', '11L98V5hcZKls', NULL, NULL, NULL, 0),
(58, 'asdasdasd', '78vQ2aH.4eCqc', '37.1978271724370', '-3.6253539802018', '2019-04-21 18:27:47', 0),
(59, 'asdasasdasd', '78vQ2aH.4eCqc', '0.0000000000000', '0.0000000000000', '2019-04-21 18:28:16', 0),
(60, 'asdasdasdddddd', '78vQ2aH.4eCqc', '37.1978271724370', '-3.6253539802018', '2019-04-21 18:29:31', 0),
(79, 'sdfsf', '1fOd1HemWQecQ', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:26:38', 0),
(80, 'asdfggg', 'ebBIHuuOkQXPQ', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:27:40', 0),
(81, 'awawdawdd', '1fOd1HemWQecQ', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:28:44', 0),
(82, 'awdawdwawd', '820RRgvgO6Xxs', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:29:21', 0),
(83, 'jkljljkl', 'b5yAc5RayjLTs', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:38:01', 0),
(84, 'hjklhjk', 'ea5Um0.SJVw1U', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:40:22', 0),
(85, 'awdawd', 'daZgJsoizQ2DA', '37.1978271724370', '-3.6253539802018', '2019-04-21 20:41:48', 0),
(86, 'gjhjgjg', '78vQ2aH.4eCqc', '0.0000000000000', '0.0000000000000', '2019-04-21 20:43:27', 0),
(87, 'prueba2', '96/kVcb7h5XRs', '37.1986171625250', '-3.6263169802018', '2019-06-18 16:39:16', 6),
(89, 'sadasd', 'a8m2xxIOcFss.', NULL, NULL, NULL, 0),
(92, 'prueba3', '5ap.V3FZA4vCI', '37.1978410000000', '-3.6238480000000', '2019-06-05 02:53:39', 0),
(93, 'prueba4', 'aa9KH9iPglt9.', '37.1978271724370', '-3.6253539802018', '2019-05-09 02:32:19', 0),
(94, 'prueba5', 'fd5nUKtu6XqHQ', '0.0000000000000', '0.0000000000000', '2019-05-06 18:41:52', 0),
(95, 'prueba6', 'd4wiR4dGagKdE', '37.1978410000000', '-3.6243510000000', '2019-06-18 16:37:56', 0),
(96, 'prueba7', '15y3fjaOvEJTg', '0.0000000000000', '0.0000000000000', '2019-06-18 16:58:40', 0),
(97, 'prueba17', 'b03mt6FV0WuLM', '0.0000000000000', '0.0000000000000', '2019-06-15 00:09:02', 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `accepted_quest`
--
ALTER TABLE `accepted_quest`
  ADD PRIMARY KEY (`id`),
  ADD KEY `quest_id` (`quest_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indices de la tabla `boss_enemy`
--
ALTER TABLE `boss_enemy`
  ADD PRIMARY KEY (`id`),
  ADD KEY `enemy_id` (`enemy_id`);

--
-- Indices de la tabla `chat_line`
--
ALTER TABLE `chat_line`
  ADD PRIMARY KEY (`id`),
  ADD KEY `second_player_id` (`second_player_id`),
  ADD KEY `owner` (`owner`);

--
-- Indices de la tabla `delivered_quest`
--
ALTER TABLE `delivered_quest`
  ADD PRIMARY KEY (`id`),
  ADD KEY `quest_id` (`quest_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indices de la tabla `enemy`
--
ALTER TABLE `enemy`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `faction`
--
ALTER TABLE `faction`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `guild`
--
ALTER TABLE `guild`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`id`),
  ADD KEY `player_id` (`player_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indices de la tabla `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`id`),
  ADD KEY `accessory` (`accessory`),
  ADD KEY `weapon` (`weapon`),
  ADD KEY `user_id` (`user_id`);

--
-- Indices de la tabla `player_battle`
--
ALTER TABLE `player_battle`
  ADD PRIMARY KEY (`id`),
  ADD KEY `first_player` (`first_player`),
  ADD KEY `second_player` (`second_player`);

--
-- Indices de la tabla `player_battle_stats`
--
ALTER TABLE `player_battle_stats`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `player_skill`
--
ALTER TABLE `player_skill`
  ADD PRIMARY KEY (`id`),
  ADD KEY `player_id` (`player_id`),
  ADD KEY `skill_id` (`skill_id`);

--
-- Indices de la tabla `quest`
--
ALTER TABLE `quest`
  ADD PRIMARY KEY (`id`),
  ADD KEY `enemy_id` (`enemy_id`);

--
-- Indices de la tabla `shop`
--
ALTER TABLE `shop`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `shop_item`
--
ALTER TABLE `shop_item`
  ADD PRIMARY KEY (`id`),
  ADD KEY `shop_id` (`shop_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indices de la tabla `skill`
--
ALTER TABLE `skill`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `accepted_quest`
--
ALTER TABLE `accepted_quest`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `boss_enemy`
--
ALTER TABLE `boss_enemy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `chat_line`
--
ALTER TABLE `chat_line`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `delivered_quest`
--
ALTER TABLE `delivered_quest`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `enemy`
--
ALTER TABLE `enemy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `faction`
--
ALTER TABLE `faction`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `guild`
--
ALTER TABLE `guild`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inventory`
--
ALTER TABLE `inventory`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT de la tabla `item`
--
ALTER TABLE `item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `player`
--
ALTER TABLE `player`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `player_battle`
--
ALTER TABLE `player_battle`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `player_battle_stats`
--
ALTER TABLE `player_battle_stats`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `player_skill`
--
ALTER TABLE `player_skill`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `quest`
--
ALTER TABLE `quest`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `shop`
--
ALTER TABLE `shop`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `shop_item`
--
ALTER TABLE `shop_item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `skill`
--
ALTER TABLE `skill`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=98;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `accepted_quest`
--
ALTER TABLE `accepted_quest`
  ADD CONSTRAINT `accepted_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`),
  ADD CONSTRAINT `accepted_quest_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`);

--
-- Filtros para la tabla `boss_enemy`
--
ALTER TABLE `boss_enemy`
  ADD CONSTRAINT `boss_enemy_ibfk_1` FOREIGN KEY (`enemy_id`) REFERENCES `enemy` (`id`);

--
-- Filtros para la tabla `chat_line`
--
ALTER TABLE `chat_line`
  ADD CONSTRAINT `chat_line_ibfk_1` FOREIGN KEY (`second_player_id`) REFERENCES `player` (`id`),
  ADD CONSTRAINT `chat_line_ibfk_2` FOREIGN KEY (`owner`) REFERENCES `player` (`id`);

--
-- Filtros para la tabla `delivered_quest`
--
ALTER TABLE `delivered_quest`
  ADD CONSTRAINT `delivered_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`),
  ADD CONSTRAINT `delivered_quest_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`);

--
-- Filtros para la tabla `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_1` FOREIGN KEY (`player_id`) REFERENCES `player` (`id`),
  ADD CONSTRAINT `inventory_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`);

--
-- Filtros para la tabla `player`
--
ALTER TABLE `player`
  ADD CONSTRAINT `player_ibfk_1` FOREIGN KEY (`accessory`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `player_ibfk_2` FOREIGN KEY (`weapon`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `player_ibfk_3` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`);

--
-- Filtros para la tabla `player_battle`
--
ALTER TABLE `player_battle`
  ADD CONSTRAINT `player_battle_ibfk_1` FOREIGN KEY (`first_player`) REFERENCES `player` (`id`),
  ADD CONSTRAINT `player_battle_ibfk_2` FOREIGN KEY (`second_player`) REFERENCES `player` (`id`);

--
-- Filtros para la tabla `player_skill`
--
ALTER TABLE `player_skill`
  ADD CONSTRAINT `player_skill_ibfk_1` FOREIGN KEY (`player_id`) REFERENCES `player` (`id`),
  ADD CONSTRAINT `player_skill_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skill` (`id`);

--
-- Filtros para la tabla `quest`
--
ALTER TABLE `quest`
  ADD CONSTRAINT `quest_ibfk_1` FOREIGN KEY (`enemy_id`) REFERENCES `enemy` (`id`);

--
-- Filtros para la tabla `shop_item`
--
ALTER TABLE `shop_item`
  ADD CONSTRAINT `item_id` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `shop_id` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`);
--
-- Base de datos: `phpmyadmin`
--
CREATE DATABASE IF NOT EXISTS `phpmyadmin` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `phpmyadmin`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__bookmark`
--

CREATE TABLE `pma__bookmark` (
  `id` int(11) NOT NULL,
  `dbase` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT '',
  `user` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT '',
  `label` varchar(255) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `query` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Bookmarks';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__central_columns`
--

CREATE TABLE `pma__central_columns` (
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `col_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `col_type` varchar(64) COLLATE utf8_bin NOT NULL,
  `col_length` text COLLATE utf8_bin,
  `col_collation` varchar(64) COLLATE utf8_bin NOT NULL,
  `col_isNull` tinyint(1) NOT NULL,
  `col_extra` varchar(255) COLLATE utf8_bin DEFAULT '',
  `col_default` text COLLATE utf8_bin
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Central list of columns';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__column_info`
--

CREATE TABLE `pma__column_info` (
  `id` int(5) UNSIGNED NOT NULL,
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `column_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `comment` varchar(255) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `mimetype` varchar(255) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `transformation` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT '',
  `transformation_options` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT '',
  `input_transformation` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT '',
  `input_transformation_options` varchar(255) COLLATE utf8_bin NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Column information for phpMyAdmin';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__designer_settings`
--

CREATE TABLE `pma__designer_settings` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `settings_data` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Settings related to Designer';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__export_templates`
--

CREATE TABLE `pma__export_templates` (
  `id` int(5) UNSIGNED NOT NULL,
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `export_type` varchar(10) COLLATE utf8_bin NOT NULL,
  `template_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `template_data` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Saved export templates';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__favorite`
--

CREATE TABLE `pma__favorite` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `tables` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Favorite tables';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__history`
--

CREATE TABLE `pma__history` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `username` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `db` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `table` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `timevalue` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `sqlquery` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='SQL history for phpMyAdmin';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__navigationhiding`
--

CREATE TABLE `pma__navigationhiding` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `item_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `item_type` varchar(64) COLLATE utf8_bin NOT NULL,
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Hidden items of navigation tree';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__pdf_pages`
--

CREATE TABLE `pma__pdf_pages` (
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `page_nr` int(10) UNSIGNED NOT NULL,
  `page_descr` varchar(50) CHARACTER SET utf8 NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='PDF relation pages for phpMyAdmin';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__recent`
--

CREATE TABLE `pma__recent` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `tables` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Recently accessed tables';

--
-- Volcado de datos para la tabla `pma__recent`
--

INSERT INTO `pma__recent` (`username`, `tables`) VALUES
('root', '[{\"db\":\"gamedb\",\"table\":\"player\"},{\"db\":\"gamedb\",\"table\":\"player_skill\"},{\"db\":\"gamedb\",\"table\":\"skill\"},{\"db\":\"gamedb\",\"table\":\"item\"},{\"db\":\"gamedb\",\"table\":\"user\"},{\"db\":\"gamedb\",\"table\":\"player_battle_stats\"},{\"db\":\"gamedb\",\"table\":\"boss_enemy\"},{\"db\":\"gamedb\",\"table\":\"player_battle\"},{\"db\":\"gamedb\",\"table\":\"accepted_quest\"},{\"db\":\"gamedb\",\"table\":\"guild\"}]');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__relation`
--

CREATE TABLE `pma__relation` (
  `master_db` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `master_table` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `master_field` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `foreign_db` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `foreign_table` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `foreign_field` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Relation table';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__savedsearches`
--

CREATE TABLE `pma__savedsearches` (
  `id` int(5) UNSIGNED NOT NULL,
  `username` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `search_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `search_data` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Saved searches';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__table_coords`
--

CREATE TABLE `pma__table_coords` (
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `pdf_page_number` int(11) NOT NULL DEFAULT '0',
  `x` float UNSIGNED NOT NULL DEFAULT '0',
  `y` float UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Table coordinates for phpMyAdmin PDF output';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__table_info`
--

CREATE TABLE `pma__table_info` (
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT '',
  `display_field` varchar(64) COLLATE utf8_bin NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Table information for phpMyAdmin';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__table_uiprefs`
--

CREATE TABLE `pma__table_uiprefs` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `prefs` text COLLATE utf8_bin NOT NULL,
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Tables'' UI preferences';

--
-- Volcado de datos para la tabla `pma__table_uiprefs`
--

INSERT INTO `pma__table_uiprefs` (`username`, `db_name`, `table_name`, `prefs`, `last_update`) VALUES
('root', 'gamedb', 'user', '{\"sorted_col\":\"`id` ASC\"}', '2019-06-13 22:00:53');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__tracking`
--

CREATE TABLE `pma__tracking` (
  `db_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `table_name` varchar(64) COLLATE utf8_bin NOT NULL,
  `version` int(10) UNSIGNED NOT NULL,
  `date_created` datetime NOT NULL,
  `date_updated` datetime NOT NULL,
  `schema_snapshot` text COLLATE utf8_bin NOT NULL,
  `schema_sql` text COLLATE utf8_bin,
  `data_sql` longtext COLLATE utf8_bin,
  `tracking` set('UPDATE','REPLACE','INSERT','DELETE','TRUNCATE','CREATE DATABASE','ALTER DATABASE','DROP DATABASE','CREATE TABLE','ALTER TABLE','RENAME TABLE','DROP TABLE','CREATE INDEX','DROP INDEX','CREATE VIEW','ALTER VIEW','DROP VIEW') COLLATE utf8_bin DEFAULT NULL,
  `tracking_active` int(1) UNSIGNED NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Database changes tracking for phpMyAdmin';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__userconfig`
--

CREATE TABLE `pma__userconfig` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `timevalue` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `config_data` text COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='User preferences storage for phpMyAdmin';

--
-- Volcado de datos para la tabla `pma__userconfig`
--

INSERT INTO `pma__userconfig` (`username`, `timevalue`, `config_data`) VALUES
('', '2019-06-07 16:40:50', '{\"lang\":\"es\"}'),
('root', '2019-06-18 15:30:24', '{\"lang\":\"es\",\"Console\\/Mode\":\"collapse\"}');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__usergroups`
--

CREATE TABLE `pma__usergroups` (
  `usergroup` varchar(64) COLLATE utf8_bin NOT NULL,
  `tab` varchar(64) COLLATE utf8_bin NOT NULL,
  `allowed` enum('Y','N') COLLATE utf8_bin NOT NULL DEFAULT 'N'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='User groups with configured menu items';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pma__users`
--

CREATE TABLE `pma__users` (
  `username` varchar(64) COLLATE utf8_bin NOT NULL,
  `usergroup` varchar(64) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='Users and their assignments to user groups';

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `pma__bookmark`
--
ALTER TABLE `pma__bookmark`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pma__central_columns`
--
ALTER TABLE `pma__central_columns`
  ADD PRIMARY KEY (`db_name`,`col_name`);

--
-- Indices de la tabla `pma__column_info`
--
ALTER TABLE `pma__column_info`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `db_name` (`db_name`,`table_name`,`column_name`);

--
-- Indices de la tabla `pma__designer_settings`
--
ALTER TABLE `pma__designer_settings`
  ADD PRIMARY KEY (`username`);

--
-- Indices de la tabla `pma__export_templates`
--
ALTER TABLE `pma__export_templates`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `u_user_type_template` (`username`,`export_type`,`template_name`);

--
-- Indices de la tabla `pma__favorite`
--
ALTER TABLE `pma__favorite`
  ADD PRIMARY KEY (`username`);

--
-- Indices de la tabla `pma__history`
--
ALTER TABLE `pma__history`
  ADD PRIMARY KEY (`id`),
  ADD KEY `username` (`username`,`db`,`table`,`timevalue`);

--
-- Indices de la tabla `pma__navigationhiding`
--
ALTER TABLE `pma__navigationhiding`
  ADD PRIMARY KEY (`username`,`item_name`,`item_type`,`db_name`,`table_name`);

--
-- Indices de la tabla `pma__pdf_pages`
--
ALTER TABLE `pma__pdf_pages`
  ADD PRIMARY KEY (`page_nr`),
  ADD KEY `db_name` (`db_name`);

--
-- Indices de la tabla `pma__recent`
--
ALTER TABLE `pma__recent`
  ADD PRIMARY KEY (`username`);

--
-- Indices de la tabla `pma__relation`
--
ALTER TABLE `pma__relation`
  ADD PRIMARY KEY (`master_db`,`master_table`,`master_field`),
  ADD KEY `foreign_field` (`foreign_db`,`foreign_table`);

--
-- Indices de la tabla `pma__savedsearches`
--
ALTER TABLE `pma__savedsearches`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `u_savedsearches_username_dbname` (`username`,`db_name`,`search_name`);

--
-- Indices de la tabla `pma__table_coords`
--
ALTER TABLE `pma__table_coords`
  ADD PRIMARY KEY (`db_name`,`table_name`,`pdf_page_number`);

--
-- Indices de la tabla `pma__table_info`
--
ALTER TABLE `pma__table_info`
  ADD PRIMARY KEY (`db_name`,`table_name`);

--
-- Indices de la tabla `pma__table_uiprefs`
--
ALTER TABLE `pma__table_uiprefs`
  ADD PRIMARY KEY (`username`,`db_name`,`table_name`);

--
-- Indices de la tabla `pma__tracking`
--
ALTER TABLE `pma__tracking`
  ADD PRIMARY KEY (`db_name`,`table_name`,`version`);

--
-- Indices de la tabla `pma__userconfig`
--
ALTER TABLE `pma__userconfig`
  ADD PRIMARY KEY (`username`);

--
-- Indices de la tabla `pma__usergroups`
--
ALTER TABLE `pma__usergroups`
  ADD PRIMARY KEY (`usergroup`,`tab`,`allowed`);

--
-- Indices de la tabla `pma__users`
--
ALTER TABLE `pma__users`
  ADD PRIMARY KEY (`username`,`usergroup`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `pma__bookmark`
--
ALTER TABLE `pma__bookmark`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pma__column_info`
--
ALTER TABLE `pma__column_info`
  MODIFY `id` int(5) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pma__export_templates`
--
ALTER TABLE `pma__export_templates`
  MODIFY `id` int(5) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pma__history`
--
ALTER TABLE `pma__history`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pma__pdf_pages`
--
ALTER TABLE `pma__pdf_pages`
  MODIFY `page_nr` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pma__savedsearches`
--
ALTER TABLE `pma__savedsearches`
  MODIFY `id` int(5) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- Base de datos: `test`
--
CREATE DATABASE IF NOT EXISTS `test` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `test`;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
