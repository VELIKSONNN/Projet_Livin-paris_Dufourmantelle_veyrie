
CREATE DATABASE  IF NOT EXISTS `baselivinparis` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `baselivinparis`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: baselivinparis
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `avis`
--

DROP TABLE IF EXISTS `avis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `avis` (
  `idavis` int NOT NULL,
  `nombre_étoiles` varchar(50) DEFAULT NULL,
  `retour_cuisinier` varchar(50) DEFAULT NULL,
  `retour_id_client` varchar(50) DEFAULT NULL,
  `date_avis` datetime NOT NULL,
  `commande` int NOT NULL,
  PRIMARY KEY (`idavis`),
  KEY `commande` (`commande`),
  CONSTRAINT `avis_ibfk_1` FOREIGN KEY (`commande`) REFERENCES `commande` (`commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avis`
--

LOCK TABLES `avis` WRITE;
/*!40000 ALTER TABLE `avis` DISABLE KEYS */;
/*!40000 ALTER TABLE `avis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `commande` int NOT NULL,
  `date_heure_commande` datetime DEFAULT NULL,
  `id_client` int NOT NULL,
  `id_cuisinier` int NOT NULL,
  PRIMARY KEY (`commande`),
  KEY `id_client` (`id_client`),
  KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `custommer` (`id_client`),
  CONSTRAINT `commande_ibfk_2` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES (1,'2025-02-27 10:00:00',1,28),(2,'2025-02-27 10:10:00',2,19),(3,'2025-02-27 10:20:00',3,1),(4,'2025-02-27 10:30:00',4,2),(5,'2025-02-27 10:40:00',5,2),(6,'2025-02-27 10:50:00',6,2),(7,'2025-02-27 11:00:00',7,3),(8,'2025-02-27 11:10:00',8,3),(9,'2025-02-27 11:20:00',9,3),(10,'2025-02-27 11:30:00',10,4),(11,'2025-02-27 11:40:00',11,4),(12,'2025-02-27 11:50:00',12,4),(13,'2025-02-27 12:00:00',13,1),(14,'2025-02-27 12:10:00',14,1),(15,'2025-02-27 12:20:00',15,2),(16,'2025-02-27 12:30:00',16,2),(17,'2025-02-27 12:40:00',17,3),(18,'2025-02-27 12:50:00',18,3),(19,'2025-02-27 13:00:00',19,4),(20,'2025-02-27 13:10:00',20,4),(21,'2025-04-04 15:19:47',1,5),(22,'2025-04-04 17:33:50',1,14);
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contient`
--

DROP TABLE IF EXISTS `contient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contient` (
  `id` smallint NOT NULL,
  `id_ingr` int NOT NULL,
  `quantité` int NOT NULL,
  PRIMARY KEY (`id`,`id_ingr`),
  KEY `id_ingr` (`id_ingr`),
  CONSTRAINT `contient_ibfk_1` FOREIGN KEY (`id`) REFERENCES `plat` (`id`),
  CONSTRAINT `contient_ibfk_2` FOREIGN KEY (`id_ingr`) REFERENCES `ingrédients` (`id_ingr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contient`
--

LOCK TABLES `contient` WRITE;
/*!40000 ALTER TABLE `contient` DISABLE KEYS */;
INSERT INTO `contient` VALUES (1,1,40),(1,2,50),(1,3,60),(1,4,70),(1,5,80),(1,6,90),(1,7,100),(1,8,110),(1,9,120),(1,10,130),(2,11,40),(2,12,50),(2,13,60),(2,14,70),(2,15,80),(2,16,90),(2,17,100),(2,18,110),(2,19,120),(2,20,130),(3,21,40),(3,22,50),(3,23,60),(3,24,70),(3,25,80),(3,26,90),(3,27,100),(3,28,110),(3,29,120),(3,30,130),(4,31,40),(4,32,50),(4,33,60),(4,34,70),(4,35,80),(4,36,90),(4,37,100),(4,38,110),(4,39,120),(4,40,130),(5,41,40),(5,42,50),(5,43,60),(5,44,70),(5,45,80),(5,46,90),(5,47,100),(5,48,110),(5,49,120),(5,50,130),(6,51,40),(6,52,50),(6,53,60),(6,54,70),(6,55,80),(6,56,90),(6,57,100),(6,58,110),(6,59,120),(6,60,130),(7,61,40),(7,62,50),(7,63,60),(7,64,70),(7,65,80),(7,66,90),(7,67,100),(7,68,110),(7,69,120),(7,70,130),(8,71,40),(8,72,50),(8,73,60),(8,74,70),(8,75,80),(8,76,90),(8,77,100),(8,78,110),(8,79,120),(8,80,130),(9,81,40),(9,82,50),(9,83,60),(9,84,70),(9,85,80),(9,86,90),(9,87,100),(9,88,110),(9,89,120),(9,90,130),(10,91,40),(10,92,50),(10,93,60),(10,94,70),(10,95,80),(10,96,90),(10,97,100),(10,98,110),(10,99,120),(10,100,130),(11,1,40),(11,2,50),(11,3,60),(11,4,70),(11,5,80),(11,6,90),(11,7,100),(11,8,110),(11,9,120),(11,10,130),(12,11,40),(12,12,50),(12,13,60),(12,14,70),(12,15,80),(12,16,90),(12,17,100),(12,18,110),(12,19,120),(12,20,130),(13,21,40),(13,22,50),(13,23,60),(13,24,70),(13,25,80),(13,26,90),(13,27,100),(13,28,110),(13,29,120),(13,30,130),(14,31,40),(14,32,50),(14,33,60),(14,34,70),(14,35,80),(14,36,90),(14,37,100),(14,38,110),(14,39,120),(14,40,130),(15,41,40),(15,42,50),(15,43,60),(15,44,70),(15,45,80),(15,46,90),(15,47,100),(15,48,110),(15,49,120),(15,50,130),(16,51,40),(16,52,50),(16,53,60),(16,54,70),(16,55,80),(16,56,90),(16,57,100),(16,58,110),(16,59,120),(16,60,130),(17,61,40),(17,62,50),(17,63,60),(17,64,70),(17,65,80),(17,66,90),(17,67,100),(17,68,110),(17,69,120),(17,70,130),(18,71,40),(18,72,50),(18,73,60),(18,74,70),(18,75,80),(18,76,90),(18,77,100),(18,78,110),(18,79,120),(18,80,130),(19,81,40),(19,82,50),(19,83,60),(19,84,70),(19,85,80),(19,86,90),(19,87,100),(19,88,110),(19,89,120),(19,90,130),(20,91,40),(20,92,50),(20,93,60),(20,94,70),(20,95,80),(20,96,90),(20,97,100),(20,98,110),(20,99,120),(20,100,130),(21,1,40),(21,2,50),(21,3,60),(21,4,70),(21,5,80),(21,6,90),(21,7,100),(21,8,110),(21,9,120),(21,10,130),(22,11,40),(22,12,50),(22,13,60),(22,14,70),(22,15,80),(22,16,90),(22,17,100),(22,18,110),(22,19,120),(22,20,130),(23,21,40),(23,22,50),(23,23,60),(23,24,70),(23,25,80),(23,26,90),(23,27,100),(23,28,110),(23,29,120),(23,30,130),(24,31,40),(24,32,50),(24,33,60),(24,34,70),(24,35,80),(24,36,90),(24,37,100),(24,38,110),(24,39,120),(24,40,130),(25,41,40),(25,42,50),(25,43,60),(25,44,70),(25,45,80),(25,46,90),(25,47,100),(25,48,110),(25,49,120),(25,50,130),(26,51,40),(26,52,50),(26,53,60),(26,54,70),(26,55,80),(26,56,90),(26,57,100),(26,58,110),(26,59,120),(26,60,130),(27,61,40),(27,62,50),(27,63,60),(27,64,70),(27,65,80),(27,66,90),(27,67,100),(27,68,110),(27,69,120),(27,70,130),(28,71,40),(28,72,50),(28,73,60),(28,74,70),(28,75,80),(28,76,90),(28,77,100),(28,78,110),(28,79,120),(28,80,130),(29,81,40),(29,82,50),(29,83,60),(29,84,70),(29,85,80),(29,86,90),(29,87,100),(29,88,110),(29,89,120),(29,90,130),(30,91,40),(30,92,50),(30,93,60),(30,94,70),(30,95,80),(30,96,90),(30,97,100),(30,98,110),(30,99,120),(30,100,130),(31,1,40),(31,2,50),(31,3,60),(31,4,70),(31,5,80),(31,6,90),(31,7,100),(31,8,110),(31,9,120),(31,10,130),(32,11,40),(32,12,50),(32,13,60),(32,14,70),(32,15,80),(32,16,90),(32,17,100),(32,18,110),(32,19,120),(32,20,130),(33,21,40),(33,22,50),(33,23,60),(33,24,70),(33,25,80),(33,26,90),(33,27,100),(33,28,110),(33,29,120),(33,30,130),(34,31,40),(34,32,50),(34,33,60),(34,34,70),(34,35,80),(34,36,90),(34,37,100),(34,38,110),(34,39,120),(34,40,130),(35,41,40),(35,42,50),(35,43,60),(35,44,70),(35,45,80),(35,46,90),(35,47,100),(35,48,110),(35,49,120),(35,50,130),(36,51,40),(36,52,50),(36,53,60),(36,54,70),(36,55,80),(36,56,90),(36,57,100),(36,58,110),(36,59,120),(36,60,130),(37,61,40),(37,62,50),(37,63,60),(37,64,70),(37,65,80),(37,66,90),(37,67,100),(37,68,110),(37,69,120),(37,70,130),(38,71,40),(38,72,50),(38,73,60),(38,74,70),(38,75,80),(38,76,90),(38,77,100),(38,78,110),(38,79,120),(38,80,130),(39,81,40),(39,82,50),(39,83,60),(39,84,70),(39,85,80),(39,86,90),(39,87,100),(39,88,110),(39,89,120),(39,90,130),(40,91,40),(40,92,50),(40,93,60),(40,94,70),(40,95,80),(40,96,90),(40,97,100),(40,98,110),(40,99,120),(40,100,130),(41,1,40),(41,2,50),(41,3,60),(41,4,70),(41,5,80),(41,6,90),(41,7,100),(41,8,110),(41,9,120),(41,10,130),(42,11,40),(42,12,50),(42,13,60),(42,14,70),(42,15,80),(42,16,90),(42,17,100),(42,18,110),(42,19,120),(42,20,130),(43,21,40),(43,22,50),(43,23,60),(43,24,70),(43,25,80),(43,26,90),(43,27,100),(43,28,110),(43,29,120),(43,30,130),(44,31,40),(44,32,50),(44,33,60),(44,34,70),(44,35,80),(44,36,90),(44,37,100),(44,38,110),(44,39,120),(44,40,130),(45,41,40),(45,42,50),(45,43,60),(45,44,70),(45,45,80),(45,46,90),(45,47,100),(45,48,110),(45,49,120),(45,50,130),(46,51,40),(46,52,50),(46,53,60),(46,54,70),(46,55,80),(46,56,90),(46,57,100),(46,58,110),(46,59,120),(46,60,130),(47,61,40),(47,62,50),(47,63,60),(47,64,70),(47,65,80),(47,66,90),(47,67,100),(47,68,110),(47,69,120),(47,70,130),(48,71,40),(48,72,50),(48,73,60),(48,74,70),(48,75,80),(48,76,90),(48,77,100),(48,78,110),(48,79,120),(48,80,130),(49,81,40),(49,82,50),(49,83,60),(49,84,70),(49,85,80),(49,86,90),(49,87,100),(49,88,110),(49,89,120),(49,90,130),(50,91,40),(50,92,50),(50,93,60),(50,94,70),(50,95,80),(50,96,90),(50,97,100),(50,98,110),(50,99,120),(50,100,130),(51,1,40),(51,2,50),(51,3,60),(51,4,70),(51,5,80),(51,6,90),(51,7,100),(51,8,110),(51,9,120),(51,10,130),(52,11,40),(52,12,50),(52,13,60),(52,14,70),(52,15,80),(52,16,90),(52,17,100),(52,18,110),(52,19,120),(52,20,130),(53,21,40),(53,22,50),(53,23,60),(53,24,70),(53,25,80),(53,26,90),(53,27,100),(53,28,110),(53,29,120),(53,30,130),(54,31,40),(54,32,50),(54,33,60),(54,34,70),(54,35,80),(54,36,90),(54,37,100),(54,38,110),(54,39,120),(54,40,130),(55,41,40),(55,42,50),(55,43,60),(55,44,70),(55,45,80),(55,46,90),(55,47,100),(55,48,110),(55,49,120),(55,50,130),(56,51,40),(56,52,50),(56,53,60),(56,54,70),(56,55,80),(56,56,90),(56,57,100),(56,58,110),(56,59,120),(56,60,130),(57,61,40),(57,62,50),(57,63,60),(57,64,70),(57,65,80),(57,66,90),(57,67,100),(57,68,110),(57,69,120),(57,70,130),(58,71,40),(58,72,50),(58,73,60),(58,74,70),(58,75,80),(58,76,90),(58,77,100),(58,78,110),(58,79,120),(58,80,130),(59,81,40),(59,82,50),(59,83,60),(59,84,70),(59,85,80),(59,86,90),(59,87,100),(59,88,110),(59,89,120),(59,90,130),(60,91,40),(60,92,50),(60,93,60),(60,94,70),(60,95,80),(60,96,90),(60,97,100),(60,98,110),(60,99,120),(60,100,130),(61,1,40),(61,2,50),(61,3,60),(61,4,70),(61,5,80),(61,6,90),(61,7,100),(61,8,110),(61,9,120),(61,10,130),(62,11,40),(62,12,50),(62,13,60),(62,14,70),(62,15,80),(62,16,90),(62,17,100),(62,18,110),(62,19,120),(62,20,130),(63,21,40),(63,22,50),(63,23,60),(63,24,70),(63,25,80),(63,26,90),(63,27,100),(63,28,110),(63,29,120),(63,30,130),(64,31,40),(64,32,50),(64,33,60),(64,34,70),(64,35,80),(64,36,90),(64,37,100),(64,38,110),(64,39,120),(64,40,130),(65,41,40),(65,42,50),(65,43,60),(65,44,70),(65,45,80),(65,46,90),(65,47,100),(65,48,110),(65,49,120),(65,50,130),(66,51,40),(66,52,50),(66,53,60),(66,54,70),(66,55,80),(66,56,90),(66,57,100),(66,58,110),(66,59,120),(66,60,130),(67,61,40),(67,62,50),(67,63,60),(67,64,70),(67,65,80),(67,66,90),(67,67,100),(67,68,110),(67,69,120),(67,70,130),(68,71,40),(68,72,50),(68,73,60),(68,74,70),(68,75,80),(68,76,90),(68,77,100),(68,78,110),(68,79,120),(68,80,130),(69,81,40),(69,82,50),(69,83,60),(69,84,70),(69,85,80),(69,86,90),(69,87,100),(69,88,110),(69,89,120),(69,90,130),(70,91,40),(70,92,50),(70,93,60),(70,94,70),(70,95,80),(70,96,90),(70,97,100),(70,98,110),(70,99,120),(70,100,130),(71,1,40),(71,2,50),(71,3,60),(71,4,70),(71,5,80),(71,6,90),(71,7,100),(71,8,110),(71,9,120),(71,10,130),(72,11,40),(72,12,50),(72,13,60),(72,14,70),(72,15,80),(72,16,90),(72,17,100),(72,18,110),(72,19,120),(72,20,130),(73,21,40),(73,22,50),(73,23,60),(73,24,70),(73,25,80),(73,26,90),(73,27,100),(73,28,110),(73,29,120),(73,30,130),(74,31,40),(74,32,50),(74,33,60),(74,34,70),(74,35,80),(74,36,90),(74,37,100),(74,38,110),(74,39,120),(74,40,130),(75,41,40),(75,42,50),(75,43,60),(75,44,70),(75,45,80),(75,46,90),(75,47,100),(75,48,110),(75,49,120),(75,50,130),(76,51,40),(76,52,50),(76,53,60),(76,54,70),(76,55,80),(76,56,90),(76,57,100),(76,58,110),(76,59,120),(76,60,130),(77,61,40),(77,62,50),(77,63,60),(77,64,70),(77,65,80),(77,66,90),(77,67,100),(77,68,110),(77,69,120),(77,70,130),(78,71,40),(78,72,50),(78,73,60),(78,74,70),(78,75,80),(78,76,90),(78,77,100),(78,78,110),(78,79,120),(78,80,130),(79,81,40),(79,82,50),(79,83,60),(79,84,70),(79,85,80),(79,86,90),(79,87,100),(79,88,110),(79,89,120),(79,90,130),(80,91,40),(80,92,50),(80,93,60),(80,94,70),(80,95,80),(80,96,90),(80,97,100),(80,98,110),(80,99,120),(80,100,130),(81,1,40),(81,2,50),(81,3,60),(81,4,70),(81,5,80),(81,6,90),(81,7,100),(81,8,110),(81,9,120),(81,10,130),(82,11,40),(82,12,50),(82,13,60),(82,14,70),(82,15,80),(82,16,90),(82,17,100),(82,18,110),(82,19,120),(82,20,130),(83,21,40),(83,22,50),(83,23,60),(83,24,70),(83,25,80),(83,26,90),(83,27,100),(83,28,110),(83,29,120),(83,30,130),(84,31,40),(84,32,50),(84,33,60),(84,34,70),(84,35,80),(84,36,90),(84,37,100),(84,38,110),(84,39,120),(84,40,130),(85,41,40),(85,42,50),(85,43,60),(85,44,70),(85,45,80),(85,46,90),(85,47,100),(85,48,110),(85,49,120),(85,50,130),(86,51,40),(86,52,50),(86,53,60),(86,54,70),(86,55,80),(86,56,90),(86,57,100),(86,58,110),(86,59,120),(86,60,130),(87,61,40),(87,62,50),(87,63,60),(87,64,70),(87,65,80),(87,66,90),(87,67,100),(87,68,110),(87,69,120),(87,70,130),(88,71,40),(88,72,50),(88,73,60),(88,74,70),(88,75,80),(88,76,90),(88,77,100),(88,78,110),(88,79,120),(88,80,130),(89,81,40),(89,82,50),(89,83,60),(89,84,70),(89,85,80),(89,86,90),(89,87,100),(89,88,110),(89,89,120),(89,90,130),(90,91,40),(90,92,50),(90,93,60),(90,94,70),(90,95,80),(90,96,90),(90,97,100),(90,98,110),(90,99,120),(90,100,130),(91,1,40),(91,2,50),(91,3,60),(91,4,70),(91,5,80),(91,6,90),(91,7,100),(91,8,110),(91,9,120),(91,10,130),(92,11,40),(92,12,50),(92,13,60),(92,14,70),(92,15,80),(92,16,90),(92,17,100),(92,18,110),(92,19,120),(92,20,130),(93,21,40),(93,22,50),(93,23,60),(93,24,70),(93,25,80),(93,26,90),(93,27,100),(93,28,110),(93,29,120),(93,30,130),(94,31,40),(94,32,50),(94,33,60),(94,34,70),(94,35,80),(94,36,90),(94,37,100),(94,38,110),(94,39,120),(94,40,130),(95,41,40),(95,42,50),(95,43,60),(95,44,70),(95,45,80),(95,46,90),(95,47,100),(95,48,110),(95,49,120),(95,50,130),(96,51,40),(96,52,50),(96,53,60),(96,54,70),(96,55,80),(96,56,90),(96,57,100),(96,58,110),(96,59,120),(96,60,130),(97,61,40),(97,62,50),(97,63,60),(97,64,70),(97,65,80),(97,66,90),(97,67,100),(97,68,110),(97,69,120),(97,70,130),(98,71,40),(98,72,50),(98,73,60),(98,74,70),(98,75,80),(98,76,90),(98,77,100),(98,78,110),(98,79,120),(98,80,130),(99,81,40),(99,82,50),(99,83,60),(99,84,70),(99,85,80),(99,86,90),(99,87,100),(99,88,110),(99,89,120),(99,90,130),(100,91,40),(100,92,50),(100,93,60),(100,94,70),(100,95,80),(100,96,90),(100,97,100),(100,98,110),(100,99,120),(100,100,130);
/*!40000 ALTER TABLE `contient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_cuisinier` int NOT NULL,
  `id` int NOT NULL,
  PRIMARY KEY (`id_cuisinier`),
  UNIQUE KEY `id` (`id`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`id`) REFERENCES `utilisateur` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10),(11,11),(12,12),(13,13),(14,14),(15,15),(16,16),(17,17),(18,18),(19,19),(20,20),(21,21),(22,22),(23,23),(24,24),(25,25),(26,26),(27,27),(28,28),(29,29),(30,30),(31,31),(32,32),(33,33),(34,34),(35,35),(36,36),(37,37),(38,38),(39,39),(40,40),(41,41),(42,42),(43,43),(44,44),(45,45),(46,46),(47,47),(48,48),(49,49),(50,50),(51,51),(52,52),(53,53),(54,54),(55,55),(56,56),(57,57),(58,58),(59,59),(60,60),(61,61),(62,62),(63,63),(64,64),(65,65),(66,66),(67,67),(68,68),(69,69),(70,70),(71,71),(72,72),(73,73),(74,74),(75,75),(76,76),(77,77),(78,78),(79,79),(80,80),(81,81),(82,82),(83,83),(84,84),(85,85),(86,86),(87,87),(88,88),(89,89),(90,90),(91,91),(92,92),(93,93),(94,94),(95,95),(96,96),(97,97),(98,98),(99,99),(100,100);
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `custommer`
--

DROP TABLE IF EXISTS `custommer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `custommer` (
  `id_client` int NOT NULL,
  `id` int NOT NULL,
  PRIMARY KEY (`id_client`),
  UNIQUE KEY `id` (`id`),
  CONSTRAINT `custommer_ibfk_1` FOREIGN KEY (`id`) REFERENCES `utilisateur` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `custommer`
--

LOCK TABLES `custommer` WRITE;
/*!40000 ALTER TABLE `custommer` DISABLE KEYS */;
INSERT INTO `custommer` VALUES (1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10),(11,11),(12,12),(13,13),(14,14),(15,15),(16,16),(17,17),(18,18),(19,19),(20,20),(21,21),(22,22),(23,23),(24,24),(25,25),(26,26),(27,27),(28,28),(29,29),(30,30),(31,31),(32,32),(33,33),(34,34),(35,35),(36,36),(37,37),(38,38),(39,39),(40,40),(41,41),(42,42),(43,43),(44,44),(45,45),(46,46),(47,47),(48,48),(49,49),(50,50);
/*!40000 ALTER TABLE `custommer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inclue`
--

DROP TABLE IF EXISTS `inclue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inclue` (
  `id` smallint NOT NULL,
  `id_ligne_de_commande` int NOT NULL,
  PRIMARY KEY (`id`,`id_ligne_de_commande`),
  KEY `id_ligne_de_commande` (`id_ligne_de_commande`),
  CONSTRAINT `inclue_ibfk_1` FOREIGN KEY (`id`) REFERENCES `plat` (`id`),
  CONSTRAINT `inclue_ibfk_2` FOREIGN KEY (`id_ligne_de_commande`) REFERENCES `ligne_de_commande_` (`id_ligne_de_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inclue`
--

LOCK TABLES `inclue` WRITE;
/*!40000 ALTER TABLE `inclue` DISABLE KEYS */;
INSERT INTO `inclue` VALUES (2,81),(14,81),(12,82),(12,83);
/*!40000 ALTER TABLE `inclue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingrédients`
--

DROP TABLE IF EXISTS `ingrédients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingrédients` (
  `id_ingr` int NOT NULL,
  `nom_ingr` varchar(50) DEFAULT NULL,
  `prix_kg` decimal(15,2) NOT NULL,
  `idpays` int NOT NULL,
  PRIMARY KEY (`id_ingr`),
  KEY `idpays` (`idpays`),
  CONSTRAINT `ingrédients_ibfk_1` FOREIGN KEY (`idpays`) REFERENCES `pays` (`idpays`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingrédients`
--

LOCK TABLES `ingrédients` WRITE;
/*!40000 ALTER TABLE `ingrédients` DISABLE KEYS */;
INSERT INTO `ingrédients` VALUES (1,'Tomate',2.50,1),(2,'Mozzarella',5.00,2),(3,'Olive',4.00,3),(4,'Farine de Blé',1.20,1),(5,'Beurre',3.50,1),(6,'Oignon',1.00,1),(7,'Ail',2.00,1),(8,'Pomme de Terre',1.10,4),(9,'Riz Basmati',2.80,7),(10,'Bœuf',9.50,1),(11,'Crevette',12.00,3),(12,'Champignon',4.50,1),(13,'Poulet',6.80,1),(14,'Poivron Rouge',3.10,1),(15,'Parmesan',8.50,2),(16,'Jambon Cru',9.00,2),(17,'Chorizo',7.20,3),(18,'Saumon',14.50,1),(19,'Fromage à la Crème',6.00,5),(20,'Lait de Coco',3.20,9),(21,'Bacon',6.40,6),(22,'Sirop d’Érable',10.00,5),(23,'Cacao en Poudre',7.50,6),(24,'Crevettes Roses',13.00,9),(25,'Fraise',4.00,1),(26,'Sucre',1.90,1),(27,'Sel de Mer',1.00,1),(28,'Huile d’Olive',6.20,3),(29,'Concombre',1.50,1),(30,'Raisin',3.60,1),(31,'Piment',4.10,9),(32,'Haricot Rouge',2.80,9),(33,'Farine de Maïs',2.20,6),(34,'Nouilles de Riz',3.00,7),(35,'Tofu Ferme',2.70,7),(36,'Gingembre',5.00,7),(37,'Poisson Blanc',9.30,3),(38,'Wasabi',20.00,8),(39,'Algues Nori',15.00,8),(40,'Sauce Soja',2.50,7),(41,'Champignon Noir',7.60,7),(42,'Pois Chiche',1.70,10),(43,'Cannelle',10.00,10),(44,'Cumin',12.00,10),(45,'Citron',3.20,1),(46,'Basilic',12.00,2),(47,'Menthe',8.60,10),(48,'Oeuf',2.00,1),(49,'Yaourt Nature',2.50,1),(50,'Miel',8.00,1),(51,'Échalote',2.40,1),(52,'Vin Blanc',4.70,1),(53,'Vin Rouge',5.20,1),(54,'Fumet de Poisson',6.60,1),(55,'Crème Fraîche',4.00,1),(56,'Pâte Feuilletée',3.80,1),(57,'Pâte Brisée',3.60,1),(58,'Nutella',8.50,6),(59,'Banane',2.10,1),(60,'Avocat',5.00,9),(61,'Haricot Vert',1.90,1),(62,'Carotte',1.20,1),(63,'Piment d\'Espelette',12.50,3),(64,'Tabasco',9.00,6),(65,'Fromage Cheddar',7.50,6),(66,'Huile de Sésame',6.70,7),(67,'Châtaigne d\'Eau',5.50,7),(68,'Vinaigre de Riz',3.20,8),(69,'Farine de Riz',2.10,7),(70,'Cassonade',2.50,1),(71,'Beurre de Cacahuète',4.80,6),(72,'Câpres',6.00,3),(73,'Céleri',1.60,1),(74,'Moutarde',2.30,1),(75,'Poudre d\'Amande',9.60,1),(76,'Fève de Cacao',15.00,6),(77,'Clou de Girofle',14.00,10),(78,'Pâte à Pizza',2.60,2),(79,'Ricotta',5.40,2),(80,'Mascarpone',6.80,2),(81,'Thon',13.00,8),(82,'Crevettes Grises',14.00,3),(83,'Poulet Fumé',8.20,6),(84,'Laitue',1.30,1),(85,'Navet',1.00,1),(86,'Radis',2.00,1),(87,'Fraise des Bois',10.00,1),(88,'Pâte Miso',7.50,8),(89,'Épices Cajun',12.00,6),(90,'Noix de Coco Râpée',5.00,9),(91,'Fenouil',3.40,1),(92,'Pois Cassés',2.50,5),(93,'Blanc d\'Œuf',2.80,1),(94,'Gelée Royale',25.00,1),(95,'Paprika',8.20,3),(96,'Origan',12.00,2),(97,'Romarin',11.00,1),(98,'Thym',10.00,1),(99,'Zeste de Citron',8.50,1),(100,'Petit Pois',1.90,1);
/*!40000 ALTER TABLE `ingrédients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ligne_de_commande_`
--

DROP TABLE IF EXISTS `ligne_de_commande_`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne_de_commande_` (
  `id_ligne_de_commande` int NOT NULL,
  `id_livraison` int NOT NULL,
  `id_client` int NOT NULL,
  `commande` int NOT NULL,
  PRIMARY KEY (`id_ligne_de_commande`),
  KEY `id_livraison` (`id_livraison`),
  KEY `id_client` (`id_client`),
  KEY `commande` (`commande`),
  CONSTRAINT `ligne_de_commande__ibfk_1` FOREIGN KEY (`id_livraison`) REFERENCES `livraison` (`id_livraison`),
  CONSTRAINT `ligne_de_commande__ibfk_2` FOREIGN KEY (`id_client`) REFERENCES `custommer` (`id_client`),
  CONSTRAINT `ligne_de_commande__ibfk_3` FOREIGN KEY (`commande`) REFERENCES `commande` (`commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ligne_de_commande_`
--

LOCK TABLES `ligne_de_commande_` WRITE;
/*!40000 ALTER TABLE `ligne_de_commande_` DISABLE KEYS */;
INSERT INTO `ligne_de_commande_` VALUES (1,1,1,1),(2,1,1,1),(3,1,1,1),(4,1,1,1),(5,2,2,2),(6,2,2,2),(7,2,2,2),(8,2,2,2),(9,99,3,3),(10,99,3,3),(11,99,3,3),(12,99,3,3),(13,99,4,4),(14,99,4,4),(15,99,4,4),(16,99,4,4),(17,99,5,5),(18,99,5,5),(19,99,5,5),(20,99,5,5),(21,99,6,6),(22,99,6,6),(23,99,6,6),(24,99,6,6),(25,99,7,7),(26,99,7,7),(27,99,7,7),(28,99,7,7),(29,99,8,8),(30,99,8,8),(31,99,8,8),(32,99,8,8),(33,99,9,9),(34,99,9,9),(35,99,9,9),(36,99,9,9),(37,99,10,10),(38,99,10,10),(39,99,10,10),(40,99,10,10),(41,99,11,11),(42,99,11,11),(43,99,11,11),(44,99,11,11),(45,99,12,12),(46,99,12,12),(47,99,12,12),(48,99,12,12),(49,99,13,13),(50,99,13,13),(51,99,13,13),(52,99,13,13),(53,99,14,14),(54,99,14,14),(55,99,14,14),(56,99,14,14),(57,99,15,15),(58,99,15,15),(59,99,15,15),(60,99,15,15),(61,99,16,16),(62,99,16,16),(63,99,16,16),(64,99,16,16),(65,99,17,17),(66,99,17,17),(67,99,17,17),(68,99,17,17),(69,99,18,18),(70,99,18,18),(71,99,18,18),(72,99,18,18),(73,99,19,19),(74,99,19,19),(75,99,19,19),(76,99,19,19),(77,99,20,20),(78,99,20,20),(79,99,20,20),(80,99,20,20),(81,100,1,21),(82,101,1,22),(83,102,1,22);
/*!40000 ALTER TABLE `ligne_de_commande_` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `id_livraison` int NOT NULL,
  `date_heure_livraison` datetime DEFAULT NULL,
  PRIMARY KEY (`id_livraison`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
INSERT INTO `livraison` VALUES (1,'2025-02-28 14:00:00'),(2,'2025-02-28 15:00:00'),(99,NULL),(100,'2000-01-01 00:00:00'),(101,'2025-01-01 00:00:00'),(102,'2025-02-01 00:00:00');
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pays`
--

DROP TABLE IF EXISTS `pays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pays` (
  `idpays` int NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`idpays`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pays`
--

LOCK TABLES `pays` WRITE;
/*!40000 ALTER TABLE `pays` DISABLE KEYS */;
INSERT INTO `pays` VALUES (1,'France'),(2,'Italie'),(3,'Espagne'),(4,'Allemagne'),(5,'Canada'),(6,'États-Unis'),(7,'Chine'),(8,'Japon'),(9,'Brésil'),(10,'Maroc');
/*!40000 ALTER TABLE `pays` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `id` smallint NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `photo_du_plat` varchar(50) DEFAULT NULL,
  `type_de_plat` varchar(50) DEFAULT NULL,
  `prix` varchar(50) DEFAULT NULL,
  `idpays` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idpays` (`idpays`),
  CONSTRAINT `plat_ibfk_1` FOREIGN KEY (`idpays`) REFERENCES `pays` (`idpays`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES (1,'Salade Fraîcheur',NULL,'Entrée','7.50',1),(2,'Spaghetti Bolognese',NULL,'Plat Principal','12.90',2),(3,'Tortilla Española',NULL,'Entrée','10.00',3),(4,'Schnitzel',NULL,'Plat Principal','14.50',4),(5,'Poutine Classique',NULL,'Plat Principal','9.90',5),(6,'Cheeseburger',NULL,'Plat Principal','11.50',6),(7,'Canard Laqué',NULL,'Plat Principal','18.00',7),(8,'Sushi Maki Assortis',NULL,'Plat Principal','16.20',8),(9,'Feijoada',NULL,'Plat Principal','13.70',9),(10,'Tajine de Poulet',NULL,'Plat Principal','15.80',10),(11,'Soupe à l\'Oignon',NULL,'Entrée','6.50',1),(12,'Tiramisu',NULL,'Dessert','5.90',2),(13,'Gazpacho',NULL,'Entrée','5.50',3),(14,'Bretzel Géant',NULL,'Amuse-Bouche','3.90',4),(15,'Tourtière Québécoise',NULL,'Plat Principal','13.40',5),(16,'Brownie au Chocolat',NULL,'Dessert','4.80',6),(17,'Riz Cantonais',NULL,'Plat Principal','8.20',7),(18,'Tempura de Légumes',NULL,'Entrée','9.50',8),(19,'Brigadeiro',NULL,'Dessert','4.20',9),(20,'Couscous Royal',NULL,'Plat Principal','14.90',10),(21,'Quiche Lorraine',NULL,'Entrée','6.90',1),(22,'Pizza Margherita',NULL,'Plat Principal','9.50',2),(23,'Paella',NULL,'Plat Principal','15.00',3),(24,'Strudel aux Pommes',NULL,'Dessert','5.10',4),(25,'Sirop d\'Érable sur Crêpe',NULL,'Dessert','4.50',5),(26,'Hot Dog Américain',NULL,'Amuse-Bouche','5.90',6),(27,'Nouilles Sautées',NULL,'Plat Principal','7.80',7),(28,'Ramen',NULL,'Plat Principal','10.90',8),(29,'Coxinha',NULL,'Amuse-Bouche','3.80',9),(30,'Pastilla',NULL,'Plat Principal','12.00',10),(31,'Crêpe Suzette',NULL,'Dessert','5.70',1),(32,'Risotto au Champignon',NULL,'Plat Principal','11.50',2),(33,'Churros',NULL,'Dessert','4.50',3),(34,'Eisbein',NULL,'Plat Principal','14.00',4),(35,'Mac & Cheese',NULL,'Plat Principal','6.50',6),(36,'Canard Pékinois',NULL,'Plat Principal','19.90',7),(37,'Sashimi de Saumon',NULL,'Entrée','17.00',8),(38,'Moqueca de Poisson',NULL,'Plat Principal','15.50',9),(39,'Harira',NULL,'Entrée','8.40',10),(40,'Bœuf Bourguignon',NULL,'Plat Principal','16.00',1),(41,'Ratatouille',NULL,'Plat Principal','8.30',1),(42,'Lasagnes Maison',NULL,'Plat Principal','13.90',2),(43,'Pulpo a la Gallega',NULL,'Entrée','14.80',3),(44,'Forêt-Noire',NULL,'Dessert','5.90',4),(45,'Soupe aux Pois',NULL,'Entrée','6.00',5),(46,'Apple Pie',NULL,'Dessert','4.50',6),(47,'Dim Sum',NULL,'Entrée','7.90',7),(48,'Okonomiyaki',NULL,'Plat Principal','12.90',8),(49,'Empadão',NULL,'Plat Principal','11.00',9),(50,'Brochette Kefta',NULL,'Plat Principal','12.60',10),(51,'Croque-Monsieur',NULL,'Entrée','5.40',1),(52,'Calzone',NULL,'Plat Principal','10.70',2),(53,'Flan Espagnol',NULL,'Dessert','4.00',3),(54,'Currywurst',NULL,'Plat Principal','4.80',4),(55,'Beignes au Sirop',NULL,'Dessert','3.50',5),(56,'Donut Chocolat',NULL,'Dessert','2.90',6),(57,'Boulettes de Porc',NULL,'Plat Principal','9.20',7),(58,'Yakitori',NULL,'Amuse-Bouche','11.20',8),(59,'Bolinho de Bacalhau',NULL,'Entrée','6.00',9),(60,'Msemen',NULL,'Amuse-Bouche','4.70',10),(61,'Tarte Tatin',NULL,'Dessert','4.60',1),(62,'Penne Arrabiata',NULL,'Plat Principal','9.30',2),(63,'Tortillas de Maïs',NULL,'Entrée','7.20',3),(64,'Spätzle Maison',NULL,'Plat Principal','10.50',4),(65,'Fèves au Lard',NULL,'Entrée','6.80',5),(66,'Bagel au Saumon',NULL,'Entrée','5.90',6),(67,'Tofu Sauce Soja',NULL,'Plat Principal','7.50',7),(68,'Takoyaki',NULL,'Amuse-Bouche','6.90',8),(69,'Pão de Queijo',NULL,'Amuse-Bouche','4.20',9),(70,'Zaalouk',NULL,'Entrée','5.10',10),(71,'Escargots au Beurre',NULL,'Entrée','7.90',1),(72,'Minestrone',NULL,'Entrée','6.40',2),(73,'Crema Catalana',NULL,'Dessert','5.00',3),(74,'Käsespätzle',NULL,'Plat Principal','11.40',4),(75,'Pâté Chinois',NULL,'Plat Principal','10.20',5),(76,'Ribs BBQ',NULL,'Plat Principal','13.80',6),(77,'Bao Zi',NULL,'Amuse-Bouche','4.70',7),(78,'Onigiri',NULL,'Amuse-Bouche','5.50',8),(79,'Vatapá',NULL,'Plat Principal','12.60',9),(80,'Mkhanfar',NULL,'Dessert','6.40',10),(81,'Blanquette de Veau',NULL,'Plat Principal','14.20',1),(82,'Gelato',NULL,'Dessert','4.60',2),(83,'Sangria Fruitée',NULL,'Amuse-Bouche','3.90',3),(84,'Leberkäse',NULL,'Plat Principal','8.90',4),(85,'Bagel Montréalais',NULL,'Amuse-Bouche','4.50',5),(86,'Milkshake Vanille',NULL,'Dessert','3.80',6),(87,'Nems au Porc',NULL,'Entrée','5.60',7),(88,'Dorayaki',NULL,'Dessert','4.20',8),(89,'Acarajé',NULL,'Entrée','5.80',9),(90,'Briouates',NULL,'Entrée','6.70',10),(91,'Gratin Dauphinois',NULL,'Plat Principal','8.80',1),(92,'Focaccia',NULL,'Amuse-Bouche','3.90',2),(93,'Tapas Variées',NULL,'Amuse-Bouche','6.50',3),(94,'Bockwurst',NULL,'Amuse-Bouche','4.50',4),(95,'Pouding Chômeur',NULL,'Dessert','3.60',5),(96,'Cheesecake',NULL,'Dessert','5.80',6),(97,'Mapo Tofu',NULL,'Plat Principal','9.70',7),(98,'Katsudon',NULL,'Plat Principal','12.00',8),(99,'Brigadeiro Blanc',NULL,'Dessert','4.20',9),(100,'Harira Royale',NULL,'Entrée','9.80',10);
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `id` int NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `tel` mediumtext NOT NULL,
  `adresse` varchar(50) NOT NULL,
  `entreprise` varchar(50) DEFAULT NULL,
  `Nom` varchar(50) NOT NULL,
  `mdp` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` (
  `id`, `Prenom`, `email`, `tel`, `adresse`, `entreprise`, `Nom`, `mdp`
) VALUES
  (  1,'Beatrice','at.iaculis@google.com','08 11 04 61 11','Porte Maillot',             NULL,'Palmer',  1),
  (  2,'Rhoda'   ,'mauris.aliquam@google.com','03 42 34 74 51','Argentine',                 NULL,'Pierce',  2),
  (  3,'Colt'    ,'posuere.cubilia@google.com','05 57 60 86 97','Charles de Gaulle - Etoile',NULL,'Chan',    3),
  (  4,'Mohammad','orci.adipiscing@google.com','04 43 34 30 15','George V',                  NULL,'Perez',   4),
  (  5,'Flavia'  ,'molestie@google.com'         ,'04 14 78 71 25','Franklin D. Roosevelt',     NULL,'Solomon', 5),
  (  6,'Amela'   ,'nec@google.com'              ,'07 23 74 25 33','Champs-Elysées - Clemenceau',NULL,'Ellis',   6),
  (  7,'Janna'   ,'pede.malesuada@google.com'   ,'04 87 81 88 22','Concorde',                  NULL,'Olson',   7),
  (  8,'Ezekiel' ,'orci@google.com'             ,'02 29 68 33 56','Tuileries',                 NULL,'Griffith',8),
  (  9,'Daria'   ,'enim.mauris.quis@google.com' ,'08 81 22 50 11','Palais Royal - Musée du Louvre',NULL,'Guerrero',9),
  ( 10,'Christian','odio.nam@google.com'        ,'03 96 13 40 57','Louvre - Rivoli',           NULL,'Wise',   10),
  ( 11,'Buckminster','nisl.quisque@google.com'  ,'08 67 52 14 47','Châtelet',                  NULL,'Langley',11),
  ( 12,'Colt'    ,'ac.turpis.egestas@google.com','05 13 93 54 80','Hôtel de Ville',            NULL,'Haney',  12),
  ( 13,'Azalia'  ,'sollicitudin.adipiscing@google.com','08 78 62 60 37','Saint-Paul (Le Marais)',NULL,'Molina',13),
  ( 14,'Evelyn'  ,'a.sollicitudin@google.com'   ,'06 13 62 92 59','Bastille',                  NULL,'Stephens',14),
  ( 15,'Guy'     ,'dolor.dolor.tempus@google.com','08 53 41 77 34','Gare de Lyon',              NULL,'Banks',  15),
  ( 16,'Gabriel' ,'consectetuer.mauris@google.com','08 14 13 81 71','Reuilly - Diderot',         NULL,'Santiago',16),
  ( 17,'Kiayada' ,'rutrum.eu@google.com'        ,'03 03 49 40 64','Nation',                    NULL,'Hayden', 17),
  ( 18,'Ingrid'  ,'aliquam@google.com'           ,'06 14 22 69 44','Porte de Vincennes',        NULL,'Rios',   18),
  ( 19,'Inez'    ,'aliquet.nec.imperdiet@google.com','08 53 56 45 23','Château de Vincennes',  NULL,'Bowers', 19),
  ( 20,'Olga'    ,'dictum.placerat.augue@google.com','07 96 32 31 64','Porte Dauphine',        NULL,'Walsh',  20),
  ( 21,'Kelly'   ,'neque.nullam.ut@google.com'   ,'04 66 10 32 87','Victor Hugo',              NULL,'Estrada',21),
  ( 22,'Breanna' ,'nonummy.ut@google.com'        ,'03 40 85 79 63','Charles de Gaulle - Etoile',NULL,'Dixon',  22),
  ( 23,'Brett'   ,'sagittis.augue@google.com'    ,'06 14 06 87 76','Ternes',                    NULL,'Hodge',  23),
  ( 24,'Tallulah','nam@google.com'               ,'06 63 37 72 68','Courcelles',                NULL,'Bray',   24),
  ( 25,'Harrison','tincidunt.dui@google.com'     ,'03 35 13 63 48','Monceau',                   NULL,'Collins',25),
  ( 26,'Geoffrey','quisque.porttitor@google.com' ,'05 13 52 54 45','Villiers',                  NULL,'Campos', 26),
  ( 27,'Kiayada' ,'vulputate.mauris@google.com'  ,'04 74 58 53 91','Rome',                      NULL,'Lang',   27),
  ( 28,'Nigel'   ,'suspendisse.sed@google.com'   ,'03 57 56 57 51','Place de Clichy',           NULL,'Flynn',  28),
  ( 29,'Colin'   ,'euismod.urna@google.com'       ,'01 06 12 89 54','Blanche',                   NULL,'Barr',   29),
  ( 30,'Palmer'  ,'nisl@google.com'              ,'05 68 51 26 24','Pigalle',                   NULL,'Carpenter',30),
  ( 31,'Kirby'   ,'dapibus@google.com'           ,'02 97 52 39 44','Anvers',                    NULL,'Stanton',31),
  ( 32,'Geoffrey','ante.lectus.convallis@google.com','07 36 19 32 61','Barbès - Rochechouart',NULL,'Hunter',32),
  ( 33,'Susan'   ,'ullamcorper.nisl@google.com'  ,'08 15 75 20 41','La Chapelle',               NULL,'Sykes',  33),
  ( 34,'Hilary'  ,'pellentesque.tincidunt@google.com','05 35 56 79 28','Stalingrad',             NULL,'Morris',34),
  ( 35,'Colt'    ,'in.consequat.enim@google.com' ,'05 95 21 12 09','Jaurès',                    NULL,'Bowen',  35),
  ( 36,'Colton'  ,'molestie.sed@google.com'      ,'04 78 44 35 14','Colonel Fabien',            NULL,'Holmes', 36),
  ( 37,'Simon'   ,'tristique.ac@google.com'      ,'01 86 62 37 72','Belleville',                NULL,'Salinas',37),
  ( 38,'Macey'   ,'bibendum.ullamcorper.duis@google.com','07 89 54 12 17','Couronnes',            NULL,'Combs',  38),
  ( 39,'Keelie'  ,'dictum.mi@google.com'         ,'09 97 70 26 69','Ménilmontant',              NULL,'Riddle',39),
  ( 40,'Lewis'   ,'sem.egestas@google.com'       ,'06 57 43 46 41','Père Lachaise',             NULL,'Benjamin',40),
  ( 41,'Charles' ,'orci.ut@google.com'           ,'06 04 19 20 89','Philippe Auguste',          NULL,'Booker',41),
  ( 42,'Rose'    ,'tempor.lorem.eget@google.com' ,'01 67 97 07 63','Alexandre Dumas',           NULL,'Hayes',  42),
  ( 43,'Bert'    ,'tristique@google.com'         ,'07 58 85 04 33','Avron',                     NULL,'Gregory',43),
  ( 44,'Jada'    ,'dolor.fusce.mi@google.com'    ,'03 11 72 38 20','Nation',                    NULL,'Cotton',44),
  ( 45,'Belle'   ,'pharetra.sed@google.com'      ,'08 27 15 16 24','Porte de Champerret',       NULL,'Christian',45),
  ( 46,'Bell'    ,'cursus.non@google.com'        ,'02 21 06 82 37','Pereire',                   NULL,'Pearson',46),
  ( 47,'Katelyn' ,'ac.nulla@google.com'          ,'04 83 61 72 37','Wagram',                    NULL,'Leonard',47),
  ( 48,'Erin'    ,'aliquet@google.com'           ,'05 96 20 53 03','Malesherbes',               NULL,'Snider',48),
  ( 49,'Aquila'  ,'porttitor@google.com'         ,'04 16 56 69 55','Villiers',                  NULL,'Goodwin',49),
  ( 50,'Chancellor','ipsum.non@google.com'      ,'04 48 78 82 72','Europe',                    NULL,'Scott',  50),
  ( 51,'Hakeem'  ,'a.neque@google.com'           ,'04 73 52 49 21','Saint-Lazare',              NULL,'Guy',    51),
  ( 52,'Tyler'   ,'aptent@google.com'            ,'07 83 37 83 57','Havre-Caumartin',           NULL,'Randolph',52),
  ( 53,'Dennis'  ,'fringilla@google.com'         ,'08 55 57 13 17','Opéra',                     NULL,'Shields',53),
  ( 54,'Warren'  ,'ornare.lectus@google.com'     ,'08 19 10 58 62','Quatre Septembre',          NULL,'Rosario',54),
  ( 55,'Dante'   ,'magna.lorem@google.com'       ,'08 53 86 91 79','Bourse',                    NULL,'Small',  55),
  ( 56,'Fritz'   ,'a@google.com'                 ,'09 64 26 40 52','Sentier',                   NULL,'Faulkner',56),
  ( 57,'Keefe'   ,'nulla.facilisis@google.com'   ,'08 04 63 87 63','Réaumur - Sébastopol',      NULL,'Rivas',  57),
  ( 58,'Ciaran'  ,'dolor.tempus@google.com'      ,'06 77 36 26 55','Arts et Métiers',           NULL,'Martinez',58),
  ( 59,'Lester'  ,'duis@google.com'              ,'03 16 65 66 20','Temple',                    NULL,'Griffin',59),
  ( 60,'Priscilla','nonummy.ipsum@google.com'    ,'08 47 46 13 32','République',                NULL,'Gill',  60),
  ( 61,'Wallace' ,'pellentesque.habitant.morbi@google.com','03 73 60 71 94','Parmentier',      NULL,'Jensen',61),
  ( 62,'Pascale' ,'lobortis@google.com'          ,'01 18 50 84 34','Rue Saint-Maur',            NULL,'Schultz',62),
  ( 63,'Kevyn'   ,'porttitor.scelerisque@google.com','05 02 33 07 73','Père Lachaise',      NULL,'O''brien',63),
  ( 64,'Jamalia' ,'mi.lacinia.mattis@google.com' ,'02 96 67 23 16','Gambetta',                  NULL,'Alexander',64),
  ( 65,'Ainsley' ,'sed.eu.nibh@google.com'       ,'07 21 86 60 28','Porte de Bagnolet',         NULL,'Sherman',65),
  ( 66,'Maxine'  ,'lorem@google.com'             ,'03 39 95 31 32','Porte des Lilas',           NULL,'Joseph',66),
  ( 67,'Gloria'  ,'lacus.pede.sagittis@google.com','09 09 53 14 95','Saint-Fargeau',      NULL,'Acosta',67),
  ( 68,'Vanna'   ,'magna.nec@google.com'         ,'08 03 71 32 33','Pelleport',                 NULL,'Moran',68),
  ( 69,'Guy'     ,'aliquet.phasellus.fermentum@google.com','02 94 68 25 18','Gambetta',    NULL,'Maxwell',69),
  ( 70,'Thor'    ,'nulla@google.com'             ,'09 86 97 67 36','Porte de Clignancourt',     NULL,'Drake',70),
  ( 71,'Sharon'  ,'adipiscing.elit.curabitur@google.com','07 46 25 34 94','Simplon',    NULL,'Duncan',71);
  
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 17:58:45
