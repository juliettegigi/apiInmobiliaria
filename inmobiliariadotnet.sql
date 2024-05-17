-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 17-05-2024 a las 20:29:49
-- Versión del servidor: 8.0.30
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariadotnet`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoriacontrato`
--

CREATE TABLE `auditoriacontrato` (
  `id` int NOT NULL,
  `id_usuario` int NOT NULL,
  `fechaInicio` datetime DEFAULT NULL,
  `fechacancelacion` datetime DEFAULT NULL,
  `id_contrato` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoriapagos`
--

CREATE TABLE `auditoriapagos` (
  `id` int NOT NULL,
  `id_usuario` int NOT NULL,
  `id_contrato` int NOT NULL,
  `fechaPago` datetime DEFAULT NULL,
  `fechaCancelacion` datetime DEFAULT NULL,
  `numero_pago` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id` int NOT NULL,
  `inquilinoId` int DEFAULT NULL,
  `inmuebleId` int DEFAULT NULL,
  `fechaInicio` datetime DEFAULT NULL,
  `fechaFin` datetime DEFAULT NULL,
  `fechaFinAnticipada` datetime DEFAULT NULL,
  `precioXmes` decimal(10,0) DEFAULT NULL,
  `estado` tinyint(1) DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imageninmuebles`
--

CREATE TABLE `imageninmuebles` (
  `id` int NOT NULL,
  `inmuebleId` int DEFAULT NULL,
  `imagen` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id` int NOT NULL,
  `propietarioId` int DEFAULT NULL,
  `inmuebleTipoId` int DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `cantidadAmbientes` int DEFAULT NULL,
  `uso` enum('Comercial','Residencial') DEFAULT NULL,
  `precioBase` decimal(10,0) DEFAULT NULL,
  `cLatitud` decimal(18,15) DEFAULT NULL,
  `CLongitud` decimal(18,15) DEFAULT NULL,
  `suspendido` tinyint(1) DEFAULT '0',
  `disponible` tinyint(1) DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id`, `propietarioId`, `inmuebleTipoId`, `direccion`, `cantidadAmbientes`, `uso`, `precioBase`, `cLatitud`, `CLongitud`, `suspendido`, `disponible`) VALUES
(1, 19, 1, 'string', 5, 'Comercial', 150000, 0.000000000000000, 0.000000000000000, 1, 1),
(2, 19, 1, '123 Main St', 3, 'Comercial', 250000, 0.000000000000000, 0.000000000000000, 0, 1),
(3, 19, 1, '123 Main St', 3, 'Comercial', 250000, 0.000000000000000, 0.000000000000000, 0, 1),
(4, 19, 2, 'dire 2', 1, 'Comercial', 1500, -33.283916504047276, 0.000000000000000, 0, 0),
(5, 19, 2, 'dire 2', 1, 'Residencial', 1500, -33.283916504047276, 0.000000000000000, 0, 0),
(6, 19, 2, 'dire 2', 1, 'Residencial', 1500, -33.283916504047276, 0.000000000000000, 0, 0),
(7, 19, 2, 'dire 2', 1, 'Residencial', 1500, -33.283916504047276, 0.000000000000000, 0, 0),
(8, 19, 2, 'dire 2', 1, 'Residencial', 1500, -33.283916504047276, 0.000000000000000, 0, 0),
(10, 19, 2, 'dire 2', 1, 'Residencial', 1500, -33.283916504047276, -66.330653726372400, 0, 0),
(11, 19, 2, 'dire 3', 1, 'Comercial', 1500, -33.283916504047276, -66.330653726372400, 0, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebletipos`
--

CREATE TABLE `inmuebletipos` (
  `id` int NOT NULL,
  `tipo` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `inmuebletipos`
--

INSERT INTO `inmuebletipos` (`id`, `tipo`) VALUES
(1, 'Local'),
(2, 'Depósito'),
(3, 'Casa'),
(4, 'Departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int NOT NULL,
  `dni` varchar(10) DEFAULT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(55) DEFAULT NULL,
  `telefono` varchar(12) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `domicilio` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `numeroPago` int NOT NULL,
  `contratoId` int NOT NULL,
  `fecha` datetime DEFAULT NULL,
  `fechaPago` datetime DEFAULT NULL,
  `importe` decimal(10,0) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int NOT NULL,
  `dni` varchar(10) DEFAULT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(55) DEFAULT NULL,
  `telefono` varchar(12) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `pass` varchar(255) DEFAULT NULL,
  `domicilio` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `pass`, `domicilio`) VALUES
(19, '35767657', 'julia', 'guti', 'prop tel 1', 'prop1@gmail.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int NOT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(55) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `pass` varchar(255) DEFAULT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `avatarFile` varchar(255) DEFAULT NULL,
  `rol` int DEFAULT NULL,
  `domicilio` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoriacontrato`
--
ALTER TABLE `auditoriacontrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_usuario` (`id_usuario`),
  ADD KEY `id_contrato` (`id_contrato`);

--
-- Indices de la tabla `auditoriapagos`
--
ALTER TABLE `auditoriapagos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_contrato` (`id_contrato`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inquilinoId` (`inquilinoId`),
  ADD KEY `inmuebleId` (`inmuebleId`);

--
-- Indices de la tabla `imageninmuebles`
--
ALTER TABLE `imageninmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inmuebleId` (`inmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `propietarioId` (`propietarioId`),
  ADD KEY `inmuebleTipoId` (`inmuebleTipoId`);

--
-- Indices de la tabla `inmuebletipos`
--
ALTER TABLE `inmuebletipos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`numeroPago`,`contratoId`),
  ADD KEY `contratoId` (`contratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoriacontrato`
--
ALTER TABLE `auditoriacontrato`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `auditoriapagos`
--
ALTER TABLE `auditoriapagos`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `imageninmuebles`
--
ALTER TABLE `imageninmuebles`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `inmuebletipos`
--
ALTER TABLE `inmuebletipos`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoriacontrato`
--
ALTER TABLE `auditoriacontrato`
  ADD CONSTRAINT `auditoriacontrato_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `auditoriacontrato_ibfk_2` FOREIGN KEY (`id_contrato`) REFERENCES `contratos` (`id`);

--
-- Filtros para la tabla `auditoriapagos`
--
ALTER TABLE `auditoriapagos`
  ADD CONSTRAINT `auditoriapagos_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contratos` (`id`),
  ADD CONSTRAINT `auditoriapagos_ibfk_2` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`);

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`inquilinoId`) REFERENCES `inquilinos` (`id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`inmuebleId`) REFERENCES `inmuebles` (`id`);

--
-- Filtros para la tabla `imageninmuebles`
--
ALTER TABLE `imageninmuebles`
  ADD CONSTRAINT `imageninmuebles_ibfk_1` FOREIGN KEY (`inmuebleId`) REFERENCES `inmuebles` (`id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`propietarioId`) REFERENCES `propietarios` (`id`),
  ADD CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`inmuebleTipoId`) REFERENCES `inmuebletipos` (`id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`contratoId`) REFERENCES `contratos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
