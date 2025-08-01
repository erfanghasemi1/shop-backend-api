-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: shop
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `orderitems`
--

DROP TABLE IF EXISTS `orderitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orderitems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrderId` int DEFAULT NULL,
  `ProductId` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `Price` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `OrderId` (`OrderId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `orderitems_ibfk_1` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`),
  CONSTRAINT `orderitems_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderitems`
--

LOCK TABLES `orderitems` WRITE;
/*!40000 ALTER TABLE `orderitems` DISABLE KEYS */;
INSERT INTO `orderitems` VALUES (1,2,1,1,135.00),(2,3,11,1,1399.00),(3,4,12,2,10.50);
/*!40000 ALTER TABLE `orderitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `BuyerId` int DEFAULT NULL,
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `BuyerId` (`BuyerId`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`BuyerId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,21,135.00,'2025-07-29 01:36:58'),(2,21,135.00,'2025-07-29 01:39:11'),(3,21,1399.00,'2025-07-29 01:41:32'),(4,24,21.00,'2025-08-01 14:19:59');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SellerId` int DEFAULT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `Description` text,
  `Price` decimal(10,2) DEFAULT NULL,
  `Stock` int DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `SellerId` (`SellerId`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`SellerId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,19,'CAT','the decryption is updated now!',10.00,6,'2025-07-22 01:23:45'),(2,19,'Cat Food','this is cat food .',135.00,5,'2025-07-22 01:23:45'),(3,19,'Dog Food','this is a greate dog food',135.00,5,'2025-07-22 01:23:45'),(4,20,'iPhone 14 Pro Max','Latest Apple smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(5,20,'iPhone 15 Pro Max','Latest Apple smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(6,20,'iPhone 16 Pro Max','Latest Apple smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(7,20,'Samsun s9 plus','Latest Samsung smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(8,20,'Samsun s10 plus','Latest Samsung smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(9,20,'Samsun s20 ultra','Latest Samsung smartphone with A16 Bionic chip',1399.00,25,'2025-07-24 18:37:15'),(10,20,'mouse','gaming mouse',1399.00,25,'2025-07-24 18:37:15'),(11,20,'asus vivobook 15','greate laptop',1399.00,24,'2025-07-24 18:37:15'),(12,22,'Mechanical Keyboard','High-quality keyboard with RGB lighting',10.50,23,'2025-07-29 14:45:00'),(13,24,'Gaming Laptop','High-performance laptop with RTX 4070',79.99,10,'2025-08-01 14:01:16');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ratings`
--

DROP TABLE IF EXISTS `ratings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ratings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProductId` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Stars` int DEFAULT NULL,
  `Comment` text,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `ProductId` (`ProductId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `ratings_ibfk_1` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`),
  CONSTRAINT `ratings_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`),
  CONSTRAINT `ratings_chk_1` CHECK ((`Stars` between 1 and 5))
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ratings`
--

LOCK TABLES `ratings` WRITE;
/*!40000 ALTER TABLE `ratings` DISABLE KEYS */;
INSERT INTO `ratings` VALUES (1,11,21,4,'Really nice quality, will buy again!','2025-07-26 18:12:21'),(2,11,21,4,'Really nice quality, will buy again!','2025-07-26 18:13:02'),(5,1,21,2,'','2025-07-26 18:18:31'),(6,1,21,2,NULL,'2025-07-26 18:18:50'),(7,1,21,2,NULL,'2025-07-26 18:19:18'),(8,10,24,5,'Amazing product! Worth every cent.','2025-08-01 14:12:54');
/*!40000 ALTER TABLE `ratings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `EncryptedPassword` varchar(255) DEFAULT NULL,
  `Role` enum('Seller','Customer') DEFAULT NULL,
  `Wallet` decimal(10,2) DEFAULT '0.00',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'erfan','erfan@gmail.com','+dB+logbUTgLDhDz6pdFUg==','Seller',0.00,'2025-07-21 14:02:51'),(2,'zahra','zahra@gmail.com','z6Cfz27xpnhraAc+9JA6Yg==','Seller',0.00,'2025-07-21 17:34:19'),(11,'','','zYx+C0Hk5uWrykQHdtEgXA==','Customer',0.00,'2025-07-22 01:28:12'),(12,'reza','reza@.com','i7dpFl5Cj5Ma379SwvktbA==','Customer',0.00,'2025-07-22 02:22:48'),(15,'ali','ali@gmail.com','CVc6SfsRKGUZNbnwv65/3Q==','Customer',0.00,'2025-07-22 02:35:41'),(16,'.','a@gmail.com','CVc6SfsRKGUZNbnwv65/3Q==','Customer',0.00,'2025-07-22 02:36:05'),(17,'..','aaa@gmail.com','CVc6SfsRKGUZNbnwv65/3Q==','Customer',0.00,'2025-07-23 02:22:37'),(18,'abc','abc@.com','sITyGIvbKtwS2KvBQB2OWg==','Seller',0.00,'2025-07-23 02:25:56'),(19,'john_doe','john@example.com','nA4JpD0YRJZDE+9EGw5mSg==','Seller',0.00,'2025-07-24 02:10:54'),(20,'e','rewa;jasdfj@example.com','nA4JpD0YRJZDE+9EGw5mSg==','Seller',0.00,'2025-07-25 14:28:03'),(21,'erfan123557755','5klj;jk@example.com','nA4JpD0YRJZDE+9EGw5mSg==','Customer',9999366.79,'2025-07-26 18:00:32'),(22,'erfan12355775','asfmf@gmail.com','nA4JpD0YRJZDE+9EGw5mSg==','Seller',0.00,'2025-07-30 04:29:09'),(23,'testuser','testuser@example.com','6a25WWAb+kuz7OtcQMoBzg==','Customer',0.00,'2025-08-01 13:53:32'),(24,'sellersam','sellersam@example.com','m+iUDe6G39rkMCjSbLYkqA==','Seller',9979.00,'2025-08-01 13:58:32');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-08-01 14:32:42
