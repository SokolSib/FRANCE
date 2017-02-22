
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/09/2014 21:36:23
-- Generated from EDMX file: D:\tfs\CASSA\cns\data\bd.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BDCA_test];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChecksTicketPayProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PayProducts] DROP CONSTRAINT [FK_ChecksTicketPayProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ChecksTicketTmpPayProductsTmp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PayProductsTmp] DROP CONSTRAINT [FK_ChecksTicketTmpPayProductsTmp];
GO
IF OBJECT_ID(N'[dbo].[FK_CloseTicketChecksTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChecksTicket] DROP CONSTRAINT [FK_CloseTicketChecksTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_CloseTicketGCloseTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CloseTicket] DROP CONSTRAINT [FK_CloseTicketGCloseTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_CloseTicketTmpChecksTicketTmp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChecksTicketTmp] DROP CONSTRAINT [FK_CloseTicketTmpChecksTicketTmp];
GO
IF OBJECT_ID(N'[dbo].[FK_EstablishmentCloseTicketG]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CloseTicketG] DROP CONSTRAINT [FK_EstablishmentCloseTicketG];
GO
IF OBJECT_ID(N'[dbo].[FK_EstablishmentStockReal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockReal] DROP CONSTRAINT [FK_EstablishmentStockReal];
GO
IF OBJECT_ID(N'[dbo].[FK_FavoriteProductsClientsInfoClients_FavoriteProductsClients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FavoriteProductsClientsInfoClients] DROP CONSTRAINT [FK_FavoriteProductsClientsInfoClients_FavoriteProductsClients];
GO
IF OBJECT_ID(N'[dbo].[FK_FavoriteProductsClientsInfoClients_InfoClients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FavoriteProductsClientsInfoClients] DROP CONSTRAINT [FK_FavoriteProductsClientsInfoClients_InfoClients];
GO
IF OBJECT_ID(N'[dbo].[FK_FavoriteProductsClientsProducts_FavoriteProductsClients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FavoriteProductsClientsProducts] DROP CONSTRAINT [FK_FavoriteProductsClientsProducts_FavoriteProductsClients];
GO
IF OBJECT_ID(N'[dbo].[FK_FavoriteProductsClientsProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FavoriteProductsClientsProducts] DROP CONSTRAINT [FK_FavoriteProductsClientsProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_FournisseurProducts_Fournisseur]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FournisseurProducts] DROP CONSTRAINT [FK_FournisseurProducts_Fournisseur];
GO
IF OBJECT_ID(N'[dbo].[FK_FournisseurProducts_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FournisseurProducts] DROP CONSTRAINT [FK_FournisseurProducts_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_GeneralEstablishment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[General] DROP CONSTRAINT [FK_GeneralEstablishment];
GO
IF OBJECT_ID(N'[dbo].[FK_GrpProductSubGrpName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubGrpNameSet] DROP CONSTRAINT [FK_GrpProductSubGrpName];
GO
IF OBJECT_ID(N'[dbo].[FK_InfoClientsCountrys]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoClients] DROP CONSTRAINT [FK_InfoClientsCountrys];
GO
IF OBJECT_ID(N'[dbo].[FK_InfoClientsOrdersWebClient_InfoClients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoClientsOrdersWebClient] DROP CONSTRAINT [FK_InfoClientsOrdersWebClient_InfoClients];
GO
IF OBJECT_ID(N'[dbo].[FK_InfoClientsOrdersWebClient_OrdersWebClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoClientsOrdersWebClient] DROP CONSTRAINT [FK_InfoClientsOrdersWebClient_OrdersWebClient];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsOrdersWebClient_OrdersWebClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsOrdersWebClient] DROP CONSTRAINT [FK_ProductsOrdersWebClient_OrdersWebClient];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsOrdersWebClient_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductsOrdersWebClient] DROP CONSTRAINT [FK_ProductsOrdersWebClient_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsProductsWeb]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_ProductsProductsWeb];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsStockReal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockReal] DROP CONSTRAINT [FK_ProductsStockReal];
GO
IF OBJECT_ID(N'[dbo].[FK_SubGrpProductProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_SubGrpProductProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_TVAProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_TVAProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_TypesPayCurrency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Currency] DROP CONSTRAINT [FK_TypesPayCurrency];
GO
IF OBJECT_ID(N'[dbo].[FK_XML_File_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[XML_File] DROP CONSTRAINT [FK_XML_File_ToTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AdminLogin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminLogin];
GO
IF OBJECT_ID(N'[dbo].[ChecksTicket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChecksTicket];
GO
IF OBJECT_ID(N'[dbo].[ChecksTicketTmp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChecksTicketTmp];
GO
IF OBJECT_ID(N'[dbo].[CloseTicket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CloseTicket];
GO
IF OBJECT_ID(N'[dbo].[CloseTicketG]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CloseTicketG];
GO
IF OBJECT_ID(N'[dbo].[CloseTicketTmp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CloseTicketTmp];
GO
IF OBJECT_ID(N'[dbo].[Countrys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countrys];
GO
IF OBJECT_ID(N'[dbo].[Currency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currency];
GO
IF OBJECT_ID(N'[dbo].[DiscountCards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiscountCards];
GO
IF OBJECT_ID(N'[dbo].[Establishment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Establishment];
GO
IF OBJECT_ID(N'[dbo].[FavoriteProductsClients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FavoriteProductsClients];
GO
IF OBJECT_ID(N'[dbo].[FavoriteProductsClientsInfoClients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FavoriteProductsClientsInfoClients];
GO
IF OBJECT_ID(N'[dbo].[FavoriteProductsClientsProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FavoriteProductsClientsProducts];
GO
IF OBJECT_ID(N'[dbo].[Fournisseur]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Fournisseur];
GO
IF OBJECT_ID(N'[dbo].[FournisseurProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FournisseurProducts];
GO
IF OBJECT_ID(N'[dbo].[General]', 'U') IS NOT NULL
    DROP TABLE [dbo].[General];
GO
IF OBJECT_ID(N'[dbo].[GrpProductSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GrpProductSet];
GO
IF OBJECT_ID(N'[dbo].[InfoClients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoClients];
GO
IF OBJECT_ID(N'[dbo].[InfoClientsOrdersWebClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoClientsOrdersWebClient];
GO
IF OBJECT_ID(N'[dbo].[LastUpd]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LastUpd];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[OpenTicketWindow]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OpenTicketWindow];
GO
IF OBJECT_ID(N'[dbo].[OrdersWebClientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrdersWebClientSet];
GO
IF OBJECT_ID(N'[dbo].[PayProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayProducts];
GO
IF OBJECT_ID(N'[dbo].[PayProductsTmp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayProductsTmp];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[ProductsAwaitingDelivery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductsAwaitingDelivery];
GO
IF OBJECT_ID(N'[dbo].[ProductsOrdersWebClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductsOrdersWebClient];
GO
IF OBJECT_ID(N'[dbo].[ProductsWeb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductsWeb];
GO
IF OBJECT_ID(N'[dbo].[StatNation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StatNation];
GO
IF OBJECT_ID(N'[dbo].[StatNationPopup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StatNationPopup];
GO
IF OBJECT_ID(N'[dbo].[StatPlaceArrond]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StatPlaceArrond];
GO
IF OBJECT_ID(N'[dbo].[StockLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockLogs];
GO
IF OBJECT_ID(N'[dbo].[StockReal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockReal];
GO
IF OBJECT_ID(N'[dbo].[SubGrpNameSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubGrpNameSet];
GO
IF OBJECT_ID(N'[dbo].[SyncPlus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SyncPlus];
GO
IF OBJECT_ID(N'[dbo].[SyncPlusProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SyncPlusProducts];
GO
IF OBJECT_ID(N'[dbo].[TranslateNameProductsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TranslateNameProductsSet];
GO
IF OBJECT_ID(N'[dbo].[TranslateUniteContenance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TranslateUniteContenance];
GO
IF OBJECT_ID(N'[dbo].[TVA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TVA];
GO
IF OBJECT_ID(N'[dbo].[TVAfuturs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TVAfuturs];
GO
IF OBJECT_ID(N'[dbo].[TypesPay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypesPay];
GO
IF OBJECT_ID(N'[dbo].[XML_File]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XML_File];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AdminLogin'
CREATE TABLE [dbo].[AdminLogin] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ChecksTicket'
CREATE TABLE [dbo].[ChecksTicket] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [BarCode] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [PayBankChecks] decimal(18,2)  NOT NULL,
    [PayBankCards] decimal(18,2)  NOT NULL,
    [PayCash] decimal(18,2)  NOT NULL,
    [PayResto] decimal(18,2)  NOT NULL,
    [Pay1] decimal(18,2)  NULL,
    [Pay2] decimal(18,2)  NULL,
    [Pay3] decimal(18,2)  NULL,
    [Pay4] decimal(18,2)  NULL,
    [Pay5] decimal(18,2)  NULL,
    [Pay6] decimal(18,2)  NULL,
    [Pay7] decimal(18,2)  NULL,
    [Pay8] decimal(18,2)  NULL,
    [Pay9] decimal(18,2)  NULL,
    [Pay10] decimal(18,2)  NULL,
    [Pay11] decimal(18,2)  NULL,
    [Pay12] decimal(18,2)  NULL,
    [Pay13] decimal(18,2)  NULL,
    [Pay14] decimal(18,2)  NULL,
    [Pay15] decimal(18,2)  NULL,
    [Pay16] decimal(18,2)  NULL,
    [Pay17] decimal(18,2)  NULL,
    [Pay18] decimal(18,2)  NULL,
    [Pay19] decimal(18,2)  NULL,
    [Pay20] decimal(18,2)  NULL,
    [CloseTicketCustumerId] uniqueidentifier  NOT NULL,
    [TotalTTC] decimal(18,2)  NULL,
    [Rendu] decimal(18,2)  NULL
);
GO

-- Creating table 'ChecksTicketTmp'
CREATE TABLE [dbo].[ChecksTicketTmp] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [BarCode] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [PayBankChecks] decimal(18,2)  NOT NULL,
    [PayBankCards] decimal(18,2)  NOT NULL,
    [PayCash] decimal(18,2)  NOT NULL,
    [PayResto] decimal(18,2)  NOT NULL,
    [Pay1] decimal(18,2)  NULL,
    [Pay2] decimal(18,2)  NULL,
    [Pay3] decimal(18,2)  NULL,
    [Pay4] decimal(18,2)  NULL,
    [Pay5] decimal(18,2)  NULL,
    [Pay6] decimal(18,2)  NULL,
    [Pay7] decimal(18,2)  NULL,
    [Pay8] decimal(18,2)  NULL,
    [Pay9] decimal(18,2)  NULL,
    [Pay10] decimal(18,2)  NULL,
    [Pay11] decimal(18,2)  NULL,
    [Pay12] decimal(18,2)  NULL,
    [Pay13] decimal(18,2)  NULL,
    [Pay14] decimal(18,2)  NULL,
    [Pay15] decimal(18,2)  NULL,
    [Pay16] decimal(18,2)  NULL,
    [Pay17] decimal(18,2)  NULL,
    [Pay18] decimal(18,2)  NULL,
    [Pay19] decimal(18,2)  NULL,
    [Pay20] decimal(18,2)  NULL,
    [TotalTTC] decimal(18,2)  NULL,
    [Rendu] decimal(18,2)  NULL,
    [CloseTicketTmpCustumerId] uniqueidentifier  NOT NULL,
    [DCBC] varchar(max)  NULL,
    [DCBC_BiloPoints] int  NULL,
    [DCBC_DobavilePoints] int  NULL,
    [DCBC_OtnayliPoints] int  NULL,
    [DCBC_OstalosPoints] int  NULL,
    [DCBC_name] varchar(max)  NULL
);
GO

-- Creating table 'CloseTicket'
CREATE TABLE [dbo].[CloseTicket] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [NameTicket] nvarchar(max)  NOT NULL,
    [DateOpen] datetime  NOT NULL,
    [DateClose] datetime  NOT NULL,
    [PayBankChecks] decimal(18,2)  NULL,
    [PayBankCards] decimal(18,2)  NULL,
    [PayCash] decimal(18,2)  NULL,
    [PayResto] decimal(18,2)  NULL,
    [Pay1] decimal(18,2)  NULL,
    [Pay2] decimal(18,2)  NULL,
    [Pay3] decimal(18,2)  NULL,
    [Pay4] decimal(18,2)  NULL,
    [Pay5] decimal(18,2)  NULL,
    [Pay6] decimal(18,2)  NULL,
    [Pay7] decimal(18,2)  NULL,
    [Pay8] decimal(18,2)  NULL,
    [Pay9] decimal(18,2)  NULL,
    [Pay10] decimal(18,2)  NULL,
    [Pay11] decimal(18,2)  NULL,
    [Pay12] decimal(18,2)  NULL,
    [Pay13] decimal(18,2)  NULL,
    [Pay14] decimal(18,2)  NULL,
    [Pay15] decimal(18,2)  NULL,
    [Pay16] decimal(18,2)  NULL,
    [Pay17] decimal(18,2)  NULL,
    [Pay18] decimal(18,2)  NULL,
    [Pay19] decimal(18,2)  NULL,
    [Pay20] decimal(18,2)  NULL,
    [CloseTicketGCustumerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'CloseTicketG'
CREATE TABLE [dbo].[CloseTicketG] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [DateOpen] datetime  NOT NULL,
    [DateClose] datetime  NOT NULL,
    [PayBankChecks] decimal(18,2)  NULL,
    [PayBankCards] decimal(18,2)  NULL,
    [PayCash] decimal(18,2)  NULL,
    [PayResto] decimal(18,2)  NULL,
    [Pay1] decimal(18,2)  NULL,
    [Pay2] decimal(18,2)  NULL,
    [Pay3] decimal(18,2)  NULL,
    [Pay4] decimal(18,2)  NULL,
    [Pay5] decimal(18,2)  NULL,
    [Pay6] decimal(18,2)  NULL,
    [Pay7] decimal(18,2)  NULL,
    [Pay8] decimal(18,2)  NULL,
    [Pay9] decimal(18,2)  NULL,
    [Pay10] decimal(18,2)  NULL,
    [Pay11] decimal(18,2)  NULL,
    [Pay12] decimal(18,2)  NULL,
    [Pay13] decimal(18,2)  NULL,
    [Pay14] decimal(18,2)  NULL,
    [Pay15] decimal(18,2)  NULL,
    [Pay16] decimal(18,2)  NULL,
    [Pay17] decimal(18,2)  NULL,
    [Pay18] decimal(18,2)  NULL,
    [Pay19] decimal(18,2)  NULL,
    [Pay20] decimal(18,2)  NULL,
    [EstablishmentCustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'CloseTicketTmp'
CREATE TABLE [dbo].[CloseTicketTmp] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [NameTicket] nvarchar(max)  NOT NULL,
    [DateOpen] datetime  NOT NULL,
    [DateClose] datetime  NOT NULL,
    [PayBankChecks] decimal(18,2)  NULL,
    [PayBankCards] decimal(18,2)  NULL,
    [PayCash] decimal(18,2)  NULL,
    [PayResto] decimal(18,2)  NULL,
    [Pay1] decimal(18,2)  NULL,
    [Pay2] decimal(18,2)  NULL,
    [Pay3] decimal(18,2)  NULL,
    [Pay4] decimal(18,2)  NULL,
    [Pay5] decimal(18,2)  NULL,
    [Pay6] decimal(18,2)  NULL,
    [Pay7] decimal(18,2)  NULL,
    [Pay8] decimal(18,2)  NULL,
    [Pay9] decimal(18,2)  NULL,
    [Pay10] decimal(18,2)  NULL,
    [Pay11] decimal(18,2)  NULL,
    [Pay12] decimal(18,2)  NULL,
    [Pay13] decimal(18,2)  NULL,
    [Pay14] decimal(18,2)  NULL,
    [Pay15] decimal(18,2)  NULL,
    [Pay16] decimal(18,2)  NULL,
    [Pay17] decimal(18,2)  NULL,
    [Pay18] decimal(18,2)  NULL,
    [Pay19] decimal(18,2)  NULL,
    [Pay20] decimal(18,2)  NULL
);
GO

-- Creating table 'Countrys'
CREATE TABLE [dbo].[Countrys] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [NameCountry] nvarchar(max)  NOT NULL,
    [Capital] nvarchar(max)  NOT NULL,
    [Continent] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Currency'
CREATE TABLE [dbo].[Currency] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Currency_money] decimal(18,2)  NOT NULL,
    [Desc] nvarchar(max)  NULL,
    [TypesPayId] int  NOT NULL
);
GO

-- Creating table 'DiscountCards'
CREATE TABLE [dbo].[DiscountCards] (
    [custumerId] uniqueidentifier  NOT NULL,
    [numberCard] char(15)  NULL,
    [points] int  NULL,
    [Active] bit  NOT NULL,
    [InfoClients_custumerId] uniqueidentifier  NULL,
    [DateTimeLastAddProduct] datetime  NULL
);
GO

-- Creating table 'Establishment'
CREATE TABLE [dbo].[Establishment] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Type] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CP] nvarchar(max)  NOT NULL,
    [Ville] nvarchar(max)  NOT NULL,
    [Adresse] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [Mail] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FavoriteProductsClients'
CREATE TABLE [dbo].[FavoriteProductsClients] (
    [CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Fournisseur'
CREATE TABLE [dbo].[Fournisseur] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [NumFour] int  NOT NULL,
    [Societe] nvarchar(max)  NULL,
    [Nom] nvarchar(max)  NULL,
    [Prenom] nvarchar(max)  NULL,
    [Adresse] nvarchar(max)  NULL,
    [CodePostal] nvarchar(max)  NULL,
    [Ville] nvarchar(max)  NULL,
    [Pays] nvarchar(max)  NULL,
    [Tel_1] nvarchar(max)  NULL,
    [Tel_2] nvarchar(max)  NULL,
    [Tel_3] nvarchar(max)  NULL,
    [Email_1] nvarchar(max)  NULL,
    [Email_2] nvarchar(max)  NULL,
    [Email_3] nvarchar(max)  NULL,
    [SiteWeb] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NULL,
    [DateCreation] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'General'
CREATE TABLE [dbo].[General] (
    [Id] uniqueidentifier  NOT NULL,
    [TicketWindowGeneral] uniqueidentifier  NULL,
    [Open] bit  NULL,
    [Name] nchar(25)  NULL,
    [User] nchar(25)  NULL,
    [Date] datetime  NULL,
    [Establishment_CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GrpProductSet'
CREATE TABLE [dbo].[GrpProductSet] (
    [Id] int  NOT NULL,
    [GroupName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InfoClients'
CREATE TABLE [dbo].[InfoClients] (
    [custumerId] uniqueidentifier  NOT NULL,
    [TypeClient] int  NOT NULL,
    [Sex] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Surname] nvarchar(max)  NULL,
    [NameCompany] nvarchar(max)  NULL,
    [SIRET] nchar(14)  NULL,
    [FRTVA] nchar(13)  NULL,
    [OfficeAddress] nvarchar(max)  NULL,
    [OfficeZipCode] nvarchar(max)  NULL,
    [OfficeCity] nvarchar(max)  NULL,
    [HomeAddress] nvarchar(max)  NULL,
    [HomeZipCode] nvarchar(max)  NULL,
    [HomeCity] nvarchar(max)  NULL,
    [Telephone] nvarchar(max)  NULL,
    [Mail] varchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [Countrys_CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'LastUpd'
CREATE TABLE [dbo].[LastUpd] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [NameTicket] nvarchar(max)  NOT NULL,
    [DateLastUpd] nvarchar(max)  NOT NULL,
    [User] nvarchar(max)  NOT NULL,
    [Upd] bit  NOT NULL,
    [IdEstablishment] uniqueidentifier  NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [Command] nvarchar(max)  NOT NULL,
    [Error] bit  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'OpenTicketWindow'
CREATE TABLE [dbo].[OpenTicketWindow] (
    [custumerId] uniqueidentifier  NOT NULL,
    [idTicketWindow] uniqueidentifier  NOT NULL,
    [nameTicket] nchar(25)  NOT NULL,
    [user] nchar(25)  NOT NULL,
    [numberTicket] int  NOT NULL,
    [isOpen] bit  NOT NULL,
    [dateOpen] datetime  NOT NULL,
    [IdTicketWindowG] uniqueidentifier  NULL,
    [Establishment_CustomerId] uniqueidentifier  NULL
);
GO

-- Creating table 'OrdersWebClientSet'
CREATE TABLE [dbo].[OrdersWebClientSet] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [TypePay] uniqueidentifier  NOT NULL,
    [CodeBar] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [QTY] decimal(18,3)  NOT NULL,
    [TVA] decimal(18,0)  NOT NULL,
    [PriceHT] nvarchar(max)  NOT NULL,
    [Total] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PayProducts'
CREATE TABLE [dbo].[PayProducts] (
    [IdCheckTicket] uniqueidentifier  NOT NULL,
    [ProductId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL,
    [QTY] decimal(18,3)  NOT NULL,
    [TVA] decimal(18,2)  NOT NULL,
    [PriceHT] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [ChecksTicketCustumerId] uniqueidentifier  NOT NULL,
    [ChecksTicketCloseTicketCustumerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PayProductsTmp'
CREATE TABLE [dbo].[PayProductsTmp] (
    [IdCheckTicket] uniqueidentifier  NOT NULL,
    [ProductId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL,
    [QTY] decimal(18,3)  NOT NULL,
    [TVA] decimal(18,2)  NOT NULL,
    [PriceHT] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [ChecksTicketTmpCustumerId] uniqueidentifier  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [sumDiscount] decimal(18,2)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CodeBare] nvarchar(max)  NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [Chp_cat] smallint  NOT NULL,
    [Balance] bit  NOT NULL,
    [Contenance] decimal(18,0)  NOT NULL,
    [UniteContenance] int  NOT NULL,
    [Tare] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [TVACustumerId] uniqueidentifier  NOT NULL,
    [ProductsWeb_CustomerId] uniqueidentifier  NOT NULL,
    [SubGrpProduct_Id] int  NOT NULL
);
GO

-- Creating table 'ProductsAwaitingDelivery'
CREATE TABLE [dbo].[ProductsAwaitingDelivery] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [OrderOrDelivery] bit  NOT NULL,
    [Validity] nvarchar(max)  NOT NULL,
    [TypeClients] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProductsWeb'
CREATE TABLE [dbo].[ProductsWeb] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Visible] bit  NOT NULL,
    [Images] nvarchar(max)  NOT NULL,
    [ContenancePallet] int  NOT NULL,
    [Weight] decimal(18,0)  NOT NULL,
    [Frozen] bit  NOT NULL
);
GO

-- Creating table 'StatNation'
CREATE TABLE [dbo].[StatNation] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Date] datetime  NOT NULL,
    [IdCaisse] uniqueidentifier  NULL,
    [IdNation] uniqueidentifier  NULL,
    [IdArrond] uniqueidentifier  NULL
);
GO

-- Creating table 'StatNationPopup'
CREATE TABLE [dbo].[StatNationPopup] (
    [IdCustomer] uniqueidentifier  NOT NULL,
    [NameNation] nvarchar(max)  NULL,
    [QTY] int  NOT NULL
);
GO

-- Creating table 'StatPlaceArrond'
CREATE TABLE [dbo].[StatPlaceArrond] (
    [IdCustomer] uniqueidentifier  NOT NULL,
    [NamePlaceArrond] nvarchar(max)  NULL,
    [QTY] int  NOT NULL
);
GO

-- Creating table 'StockLogs'
CREATE TABLE [dbo].[StockLogs] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [TypeOperation] bit  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL,
    [QTY] decimal(18,2)  NOT NULL,
    [User] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'StockReal'
CREATE TABLE [dbo].[StockReal] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [QTY] decimal(18,3)  NOT NULL,
    [MinQTY] decimal(18,0)  NOT NULL,
    [Price] decimal(18,2)  NOT NULL,
    [ProductsCustumerId] uniqueidentifier  NOT NULL,
    [Establishment_CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'SubGrpNameSet'
CREATE TABLE [dbo].[SubGrpNameSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubGroupName] nvarchar(max)  NOT NULL,
    [GrpProductId] int  NOT NULL
);
GO

-- Creating table 'SyncPlus'
CREATE TABLE [dbo].[SyncPlus] (
    [customerId] uniqueidentifier  NOT NULL,
    [date] datetime  NULL,
    [nameCasse] nvarchar(max)  NULL
);
GO

-- Creating table 'SyncPlusProducts'
CREATE TABLE [dbo].[SyncPlusProducts] (
    [customerId] uniqueidentifier  NOT NULL,
    [check] nvarchar(max)  NOT NULL,
    [date] datetime  NOT NULL,
    [customerIdSyncPlus] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'TranslateNameProductsSet'
CREATE TABLE [dbo].[TranslateNameProductsSet] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [cod_lng] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TranslateUniteContenance'
CREATE TABLE [dbo].[TranslateUniteContenance] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [IdUnite] int  NOT NULL,
    [cod_lng] nvarchar(max)  NOT NULL,
    [nameUnite] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TVA'
CREATE TABLE [dbo].[TVA] (
    [CustumerId] uniqueidentifier  NOT NULL,
    [Id] nvarchar(max)  NOT NULL,
    [val] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TVAfuturs'
CREATE TABLE [dbo].[TVAfuturs] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [Date] datetime  NOT NULL,
    [idTVA] nvarchar(max)  NOT NULL,
    [val] nvarchar(max)  NOT NULL,
    [userID] nvarchar(max)  NOT NULL,
    [active] bit  NOT NULL
);
GO

-- Creating table 'TypesPay'
CREATE TABLE [dbo].[TypesPay] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Etat] bit  NULL,
    [Name] nvarchar(max)  NULL,
    [NameCourt] nchar(10)  NULL,
    [CodeCompta] int  NULL,
    [Rendu_Avoir] bit  NULL,
    [Tiroir] bit  NULL,
    [CurMod] bit  NULL
);
GO

-- Creating table 'XML_File'
CREATE TABLE [dbo].[XML_File] (
    [CustomerId] uniqueidentifier  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Upd] bit  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NULL,
    [Establishment_CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'FavoriteProductsClientsInfoClients'
CREATE TABLE [dbo].[FavoriteProductsClientsInfoClients] (
    [FavoriteProductsClients_CustomerId] uniqueidentifier  NOT NULL,
    [InfoClients_custumerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'FavoriteProductsClientsProducts'
CREATE TABLE [dbo].[FavoriteProductsClientsProducts] (
    [FavoriteProductsClients_CustomerId] uniqueidentifier  NOT NULL,
    [Products_CustumerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'FournisseurProducts'
CREATE TABLE [dbo].[FournisseurProducts] (
    [Fournisseur_CustomerId] uniqueidentifier  NOT NULL,
    [Products_CustumerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'InfoClientsOrdersWebClient'
CREATE TABLE [dbo].[InfoClientsOrdersWebClient] (
    [InfoClients_custumerId] uniqueidentifier  NOT NULL,
    [OrdersWebClientSet_CustomerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'ProductsOrdersWebClient'
CREATE TABLE [dbo].[ProductsOrdersWebClient] (
    [OrdersWebClientSet_CustomerId] uniqueidentifier  NOT NULL,
    [Products_CustumerId] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CustomerId] in table 'AdminLogin'
ALTER TABLE [dbo].[AdminLogin]
ADD CONSTRAINT [PK_AdminLogin]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustumerId], [CloseTicketCustumerId] in table 'ChecksTicket'
ALTER TABLE [dbo].[ChecksTicket]
ADD CONSTRAINT [PK_ChecksTicket]
    PRIMARY KEY CLUSTERED ([CustumerId], [CloseTicketCustumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'ChecksTicketTmp'
ALTER TABLE [dbo].[ChecksTicketTmp]
ADD CONSTRAINT [PK_ChecksTicketTmp]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'CloseTicket'
ALTER TABLE [dbo].[CloseTicket]
ADD CONSTRAINT [PK_CloseTicket]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'CloseTicketG'
ALTER TABLE [dbo].[CloseTicketG]
ADD CONSTRAINT [PK_CloseTicketG]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'CloseTicketTmp'
ALTER TABLE [dbo].[CloseTicketTmp]
ADD CONSTRAINT [PK_CloseTicketTmp]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Countrys'
ALTER TABLE [dbo].[Countrys]
ADD CONSTRAINT [PK_Countrys]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Currency'
ALTER TABLE [dbo].[Currency]
ADD CONSTRAINT [PK_Currency]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [custumerId] in table 'DiscountCards'
ALTER TABLE [dbo].[DiscountCards]
ADD CONSTRAINT [PK_DiscountCards]
    PRIMARY KEY CLUSTERED ([custumerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Establishment'
ALTER TABLE [dbo].[Establishment]
ADD CONSTRAINT [PK_Establishment]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'FavoriteProductsClients'
ALTER TABLE [dbo].[FavoriteProductsClients]
ADD CONSTRAINT [PK_FavoriteProductsClients]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Fournisseur'
ALTER TABLE [dbo].[Fournisseur]
ADD CONSTRAINT [PK_Fournisseur]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [Id] in table 'General'
ALTER TABLE [dbo].[General]
ADD CONSTRAINT [PK_General]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GrpProductSet'
ALTER TABLE [dbo].[GrpProductSet]
ADD CONSTRAINT [PK_GrpProductSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [custumerId] in table 'InfoClients'
ALTER TABLE [dbo].[InfoClients]
ADD CONSTRAINT [PK_InfoClients]
    PRIMARY KEY CLUSTERED ([custumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'LastUpd'
ALTER TABLE [dbo].[LastUpd]
ADD CONSTRAINT [PK_LastUpd]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [custumerId] in table 'OpenTicketWindow'
ALTER TABLE [dbo].[OpenTicketWindow]
ADD CONSTRAINT [PK_OpenTicketWindow]
    PRIMARY KEY CLUSTERED ([custumerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'OrdersWebClientSet'
ALTER TABLE [dbo].[OrdersWebClientSet]
ADD CONSTRAINT [PK_OrdersWebClientSet]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [IdCheckTicket], [ChecksTicketCustumerId], [ChecksTicketCloseTicketCustumerId] in table 'PayProducts'
ALTER TABLE [dbo].[PayProducts]
ADD CONSTRAINT [PK_PayProducts]
    PRIMARY KEY CLUSTERED ([IdCheckTicket], [ChecksTicketCustumerId], [ChecksTicketCloseTicketCustumerId] ASC);
GO

-- Creating primary key on [IdCheckTicket] in table 'PayProductsTmp'
ALTER TABLE [dbo].[PayProductsTmp]
ADD CONSTRAINT [PK_PayProductsTmp]
    PRIMARY KEY CLUSTERED ([IdCheckTicket] ASC);
GO

-- Creating primary key on [CustumerId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'ProductsAwaitingDelivery'
ALTER TABLE [dbo].[ProductsAwaitingDelivery]
ADD CONSTRAINT [PK_ProductsAwaitingDelivery]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'ProductsWeb'
ALTER TABLE [dbo].[ProductsWeb]
ADD CONSTRAINT [PK_ProductsWeb]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'StatNation'
ALTER TABLE [dbo].[StatNation]
ADD CONSTRAINT [PK_StatNation]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [IdCustomer] in table 'StatNationPopup'
ALTER TABLE [dbo].[StatNationPopup]
ADD CONSTRAINT [PK_StatNationPopup]
    PRIMARY KEY CLUSTERED ([IdCustomer] ASC);
GO

-- Creating primary key on [IdCustomer] in table 'StatPlaceArrond'
ALTER TABLE [dbo].[StatPlaceArrond]
ADD CONSTRAINT [PK_StatPlaceArrond]
    PRIMARY KEY CLUSTERED ([IdCustomer] ASC);
GO

-- Creating primary key on [CustomerId] in table 'StockLogs'
ALTER TABLE [dbo].[StockLogs]
ADD CONSTRAINT [PK_StockLogs]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'StockReal'
ALTER TABLE [dbo].[StockReal]
ADD CONSTRAINT [PK_StockReal]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [Id] in table 'SubGrpNameSet'
ALTER TABLE [dbo].[SubGrpNameSet]
ADD CONSTRAINT [PK_SubGrpNameSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [customerId] in table 'SyncPlus'
ALTER TABLE [dbo].[SyncPlus]
ADD CONSTRAINT [PK_SyncPlus]
    PRIMARY KEY CLUSTERED ([customerId] ASC);
GO

-- Creating primary key on [customerId] in table 'SyncPlusProducts'
ALTER TABLE [dbo].[SyncPlusProducts]
ADD CONSTRAINT [PK_SyncPlusProducts]
    PRIMARY KEY CLUSTERED ([customerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'TranslateNameProductsSet'
ALTER TABLE [dbo].[TranslateNameProductsSet]
ADD CONSTRAINT [PK_TranslateNameProductsSet]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'TranslateUniteContenance'
ALTER TABLE [dbo].[TranslateUniteContenance]
ADD CONSTRAINT [PK_TranslateUniteContenance]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustumerId] in table 'TVA'
ALTER TABLE [dbo].[TVA]
ADD CONSTRAINT [PK_TVA]
    PRIMARY KEY CLUSTERED ([CustumerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'TVAfuturs'
ALTER TABLE [dbo].[TVAfuturs]
ADD CONSTRAINT [PK_TVAfuturs]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [Id] in table 'TypesPay'
ALTER TABLE [dbo].[TypesPay]
ADD CONSTRAINT [PK_TypesPay]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CustomerId] in table 'XML_File'
ALTER TABLE [dbo].[XML_File]
ADD CONSTRAINT [PK_XML_File]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [FavoriteProductsClients_CustomerId], [InfoClients_custumerId] in table 'FavoriteProductsClientsInfoClients'
ALTER TABLE [dbo].[FavoriteProductsClientsInfoClients]
ADD CONSTRAINT [PK_FavoriteProductsClientsInfoClients]
    PRIMARY KEY CLUSTERED ([FavoriteProductsClients_CustomerId], [InfoClients_custumerId] ASC);
GO

-- Creating primary key on [FavoriteProductsClients_CustomerId], [Products_CustumerId] in table 'FavoriteProductsClientsProducts'
ALTER TABLE [dbo].[FavoriteProductsClientsProducts]
ADD CONSTRAINT [PK_FavoriteProductsClientsProducts]
    PRIMARY KEY CLUSTERED ([FavoriteProductsClients_CustomerId], [Products_CustumerId] ASC);
GO

-- Creating primary key on [Fournisseur_CustomerId], [Products_CustumerId] in table 'FournisseurProducts'
ALTER TABLE [dbo].[FournisseurProducts]
ADD CONSTRAINT [PK_FournisseurProducts]
    PRIMARY KEY CLUSTERED ([Fournisseur_CustomerId], [Products_CustumerId] ASC);
GO

-- Creating primary key on [InfoClients_custumerId], [OrdersWebClientSet_CustomerId] in table 'InfoClientsOrdersWebClient'
ALTER TABLE [dbo].[InfoClientsOrdersWebClient]
ADD CONSTRAINT [PK_InfoClientsOrdersWebClient]
    PRIMARY KEY CLUSTERED ([InfoClients_custumerId], [OrdersWebClientSet_CustomerId] ASC);
GO

-- Creating primary key on [OrdersWebClientSet_CustomerId], [Products_CustumerId] in table 'ProductsOrdersWebClient'
ALTER TABLE [dbo].[ProductsOrdersWebClient]
ADD CONSTRAINT [PK_ProductsOrdersWebClient]
    PRIMARY KEY CLUSTERED ([OrdersWebClientSet_CustomerId], [Products_CustumerId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ChecksTicketCustumerId], [ChecksTicketCloseTicketCustumerId] in table 'PayProducts'
ALTER TABLE [dbo].[PayProducts]
ADD CONSTRAINT [FK_ChecksTicketPayProducts]
    FOREIGN KEY ([ChecksTicketCustumerId], [ChecksTicketCloseTicketCustumerId])
    REFERENCES [dbo].[ChecksTicket]
        ([CustumerId], [CloseTicketCustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChecksTicketPayProducts'
CREATE INDEX [IX_FK_ChecksTicketPayProducts]
ON [dbo].[PayProducts]
    ([ChecksTicketCustumerId], [ChecksTicketCloseTicketCustumerId]);
GO

-- Creating foreign key on [CloseTicketCustumerId] in table 'ChecksTicket'
ALTER TABLE [dbo].[ChecksTicket]
ADD CONSTRAINT [FK_CloseTicketChecksTicket]
    FOREIGN KEY ([CloseTicketCustumerId])
    REFERENCES [dbo].[CloseTicket]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CloseTicketChecksTicket'
CREATE INDEX [IX_FK_CloseTicketChecksTicket]
ON [dbo].[ChecksTicket]
    ([CloseTicketCustumerId]);
GO

-- Creating foreign key on [ChecksTicketTmpCustumerId] in table 'PayProductsTmp'
ALTER TABLE [dbo].[PayProductsTmp]
ADD CONSTRAINT [FK_ChecksTicketTmpPayProductsTmp]
    FOREIGN KEY ([ChecksTicketTmpCustumerId])
    REFERENCES [dbo].[ChecksTicketTmp]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChecksTicketTmpPayProductsTmp'
CREATE INDEX [IX_FK_ChecksTicketTmpPayProductsTmp]
ON [dbo].[PayProductsTmp]
    ([ChecksTicketTmpCustumerId]);
GO

-- Creating foreign key on [CloseTicketTmpCustumerId] in table 'ChecksTicketTmp'
ALTER TABLE [dbo].[ChecksTicketTmp]
ADD CONSTRAINT [FK_CloseTicketTmpChecksTicketTmp]
    FOREIGN KEY ([CloseTicketTmpCustumerId])
    REFERENCES [dbo].[CloseTicketTmp]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CloseTicketTmpChecksTicketTmp'
CREATE INDEX [IX_FK_CloseTicketTmpChecksTicketTmp]
ON [dbo].[ChecksTicketTmp]
    ([CloseTicketTmpCustumerId]);
GO

-- Creating foreign key on [CloseTicketGCustumerId] in table 'CloseTicket'
ALTER TABLE [dbo].[CloseTicket]
ADD CONSTRAINT [FK_CloseTicketGCloseTicket]
    FOREIGN KEY ([CloseTicketGCustumerId])
    REFERENCES [dbo].[CloseTicketG]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CloseTicketGCloseTicket'
CREATE INDEX [IX_FK_CloseTicketGCloseTicket]
ON [dbo].[CloseTicket]
    ([CloseTicketGCustumerId]);
GO

-- Creating foreign key on [EstablishmentCustomerId] in table 'CloseTicketG'
ALTER TABLE [dbo].[CloseTicketG]
ADD CONSTRAINT [FK_EstablishmentCloseTicketG]
    FOREIGN KEY ([EstablishmentCustomerId])
    REFERENCES [dbo].[Establishment]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstablishmentCloseTicketG'
CREATE INDEX [IX_FK_EstablishmentCloseTicketG]
ON [dbo].[CloseTicketG]
    ([EstablishmentCustomerId]);
GO

-- Creating foreign key on [Countrys_CustomerId] in table 'InfoClients'
ALTER TABLE [dbo].[InfoClients]
ADD CONSTRAINT [FK_InfoClientsCountrys]
    FOREIGN KEY ([Countrys_CustomerId])
    REFERENCES [dbo].[Countrys]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InfoClientsCountrys'
CREATE INDEX [IX_FK_InfoClientsCountrys]
ON [dbo].[InfoClients]
    ([Countrys_CustomerId]);
GO

-- Creating foreign key on [TypesPayId] in table 'Currency'
ALTER TABLE [dbo].[Currency]
ADD CONSTRAINT [FK_TypesPayCurrency]
    FOREIGN KEY ([TypesPayId])
    REFERENCES [dbo].[TypesPay]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypesPayCurrency'
CREATE INDEX [IX_FK_TypesPayCurrency]
ON [dbo].[Currency]
    ([TypesPayId]);
GO

-- Creating foreign key on [Establishment_CustomerId] in table 'StockReal'
ALTER TABLE [dbo].[StockReal]
ADD CONSTRAINT [FK_EstablishmentStockReal]
    FOREIGN KEY ([Establishment_CustomerId])
    REFERENCES [dbo].[Establishment]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstablishmentStockReal'
CREATE INDEX [IX_FK_EstablishmentStockReal]
ON [dbo].[StockReal]
    ([Establishment_CustomerId]);
GO

-- Creating foreign key on [Establishment_CustomerId] in table 'General'
ALTER TABLE [dbo].[General]
ADD CONSTRAINT [FK_GeneralEstablishment]
    FOREIGN KEY ([Establishment_CustomerId])
    REFERENCES [dbo].[Establishment]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GeneralEstablishment'
CREATE INDEX [IX_FK_GeneralEstablishment]
ON [dbo].[General]
    ([Establishment_CustomerId]);
GO

-- Creating foreign key on [GrpProductId] in table 'SubGrpNameSet'
ALTER TABLE [dbo].[SubGrpNameSet]
ADD CONSTRAINT [FK_GrpProductSubGrpName]
    FOREIGN KEY ([GrpProductId])
    REFERENCES [dbo].[GrpProductSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GrpProductSubGrpName'
CREATE INDEX [IX_FK_GrpProductSubGrpName]
ON [dbo].[SubGrpNameSet]
    ([GrpProductId]);
GO

-- Creating foreign key on [ProductsWeb_CustomerId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductsProductsWeb]
    FOREIGN KEY ([ProductsWeb_CustomerId])
    REFERENCES [dbo].[ProductsWeb]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsProductsWeb'
CREATE INDEX [IX_FK_ProductsProductsWeb]
ON [dbo].[Products]
    ([ProductsWeb_CustomerId]);
GO

-- Creating foreign key on [ProductsCustumerId] in table 'StockReal'
ALTER TABLE [dbo].[StockReal]
ADD CONSTRAINT [FK_ProductsStockReal]
    FOREIGN KEY ([ProductsCustumerId])
    REFERENCES [dbo].[Products]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsStockReal'
CREATE INDEX [IX_FK_ProductsStockReal]
ON [dbo].[StockReal]
    ([ProductsCustumerId]);
GO

-- Creating foreign key on [SubGrpProduct_Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_SubGrpProductProducts]
    FOREIGN KEY ([SubGrpProduct_Id])
    REFERENCES [dbo].[SubGrpNameSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubGrpProductProducts'
CREATE INDEX [IX_FK_SubGrpProductProducts]
ON [dbo].[Products]
    ([SubGrpProduct_Id]);
GO

-- Creating foreign key on [TVACustumerId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_TVAProducts]
    FOREIGN KEY ([TVACustumerId])
    REFERENCES [dbo].[TVA]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TVAProducts'
CREATE INDEX [IX_FK_TVAProducts]
ON [dbo].[Products]
    ([TVACustumerId]);
GO

-- Creating foreign key on [FavoriteProductsClients_CustomerId] in table 'FavoriteProductsClientsInfoClients'
ALTER TABLE [dbo].[FavoriteProductsClientsInfoClients]
ADD CONSTRAINT [FK_FavoriteProductsClientsInfoClients_FavoriteProductsClients]
    FOREIGN KEY ([FavoriteProductsClients_CustomerId])
    REFERENCES [dbo].[FavoriteProductsClients]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [InfoClients_custumerId] in table 'FavoriteProductsClientsInfoClients'
ALTER TABLE [dbo].[FavoriteProductsClientsInfoClients]
ADD CONSTRAINT [FK_FavoriteProductsClientsInfoClients_InfoClients]
    FOREIGN KEY ([InfoClients_custumerId])
    REFERENCES [dbo].[InfoClients]
        ([custumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FavoriteProductsClientsInfoClients_InfoClients'
CREATE INDEX [IX_FK_FavoriteProductsClientsInfoClients_InfoClients]
ON [dbo].[FavoriteProductsClientsInfoClients]
    ([InfoClients_custumerId]);
GO

-- Creating foreign key on [FavoriteProductsClients_CustomerId] in table 'FavoriteProductsClientsProducts'
ALTER TABLE [dbo].[FavoriteProductsClientsProducts]
ADD CONSTRAINT [FK_FavoriteProductsClientsProducts_FavoriteProductsClients]
    FOREIGN KEY ([FavoriteProductsClients_CustomerId])
    REFERENCES [dbo].[FavoriteProductsClients]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Products_CustumerId] in table 'FavoriteProductsClientsProducts'
ALTER TABLE [dbo].[FavoriteProductsClientsProducts]
ADD CONSTRAINT [FK_FavoriteProductsClientsProducts_Products]
    FOREIGN KEY ([Products_CustumerId])
    REFERENCES [dbo].[Products]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FavoriteProductsClientsProducts_Products'
CREATE INDEX [IX_FK_FavoriteProductsClientsProducts_Products]
ON [dbo].[FavoriteProductsClientsProducts]
    ([Products_CustumerId]);
GO

-- Creating foreign key on [Fournisseur_CustomerId] in table 'FournisseurProducts'
ALTER TABLE [dbo].[FournisseurProducts]
ADD CONSTRAINT [FK_FournisseurProducts_Fournisseur]
    FOREIGN KEY ([Fournisseur_CustomerId])
    REFERENCES [dbo].[Fournisseur]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Products_CustumerId] in table 'FournisseurProducts'
ALTER TABLE [dbo].[FournisseurProducts]
ADD CONSTRAINT [FK_FournisseurProducts_Products]
    FOREIGN KEY ([Products_CustumerId])
    REFERENCES [dbo].[Products]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FournisseurProducts_Products'
CREATE INDEX [IX_FK_FournisseurProducts_Products]
ON [dbo].[FournisseurProducts]
    ([Products_CustumerId]);
GO

-- Creating foreign key on [InfoClients_custumerId] in table 'InfoClientsOrdersWebClient'
ALTER TABLE [dbo].[InfoClientsOrdersWebClient]
ADD CONSTRAINT [FK_InfoClientsOrdersWebClient_InfoClients]
    FOREIGN KEY ([InfoClients_custumerId])
    REFERENCES [dbo].[InfoClients]
        ([custumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OrdersWebClientSet_CustomerId] in table 'InfoClientsOrdersWebClient'
ALTER TABLE [dbo].[InfoClientsOrdersWebClient]
ADD CONSTRAINT [FK_InfoClientsOrdersWebClient_OrdersWebClientSet]
    FOREIGN KEY ([OrdersWebClientSet_CustomerId])
    REFERENCES [dbo].[OrdersWebClientSet]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InfoClientsOrdersWebClient_OrdersWebClientSet'
CREATE INDEX [IX_FK_InfoClientsOrdersWebClient_OrdersWebClientSet]
ON [dbo].[InfoClientsOrdersWebClient]
    ([OrdersWebClientSet_CustomerId]);
GO

-- Creating foreign key on [OrdersWebClientSet_CustomerId] in table 'ProductsOrdersWebClient'
ALTER TABLE [dbo].[ProductsOrdersWebClient]
ADD CONSTRAINT [FK_ProductsOrdersWebClient_OrdersWebClientSet]
    FOREIGN KEY ([OrdersWebClientSet_CustomerId])
    REFERENCES [dbo].[OrdersWebClientSet]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Products_CustumerId] in table 'ProductsOrdersWebClient'
ALTER TABLE [dbo].[ProductsOrdersWebClient]
ADD CONSTRAINT [FK_ProductsOrdersWebClient_Products]
    FOREIGN KEY ([Products_CustumerId])
    REFERENCES [dbo].[Products]
        ([CustumerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsOrdersWebClient_Products'
CREATE INDEX [IX_FK_ProductsOrdersWebClient_Products]
ON [dbo].[ProductsOrdersWebClient]
    ([Products_CustumerId]);
GO

-- Creating foreign key on [Establishment_CustomerId] in table 'XML_File'
ALTER TABLE [dbo].[XML_File]
ADD CONSTRAINT [FK_XML_FileEstablishment]
    FOREIGN KEY ([Establishment_CustomerId])
    REFERENCES [dbo].[Establishment]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_XML_FileEstablishment'
CREATE INDEX [IX_FK_XML_FileEstablishment]
ON [dbo].[XML_File]
    ([Establishment_CustomerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------