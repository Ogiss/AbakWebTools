UPDATE category SET id_local_parent = NULL WHERE id_local_parent = 0;
UPDATE category SET id_category = NULL;
UPDATE product SET id_product = NULL;
UPDATE image SET id_image = NULL;
UPDATE product SET Usuniety = 0 WHERE Usuniety IS NULL;
UPDATE product SET synchronize = 2 where Usuniety = 0 AND synchronize = 0 AND id_product IS NULL;
UPDATE [tax] SET id_tax = NULL;
UPDATE Kontrahenci SET PSID = NULL;
UPDATE Adresy SET PSID = NULL;

-- Customer

ALTER TABLE Kontrahenci ADD CreationDate DateTime2(4) NOT NULL DEFAULT (GETDATE());
ALTER TABLE Kontrahenci ADD ModificationDate DateTime2(4) NOT NULL DEFAULT (GETDATE());

GO
UPDATE Kontrahenci SET ModificationDate = CreationDate;

GO
ALTER TABLE Kontrahenci ADD WebAccountLogin VARCHAR(100);
ALTER TABLE Kontrahenci ADD WebAccountPassword VARCHAR(255);
GO
UPDATE Kontrahenci SET WebAccountLogin = LTRIM(RTRIM(Email))
 WHERE Usuniety = 0 AND Email IS NOT NULL AND LTRIM(RTRIM(Email)) <> '' 
 AND Email NOT IN (SELECT Email FROM Kontrahenci WHERE Usuniety = 0 AND Email IS NOT NULL AND Email <> '' GROUP BY Email HAVING COUNT(*) > 1);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_UNIQUE_WebAccountLogin] ON [dbo].[Kontrahenci]
(
	[WebAccountLogin] ASC
)
WHERE ([Usuniety]=(0) AND [Synchronizacja]<>(4) AND [WebAccountLogin] IS NOT NULL);
GO

-- Address

ALTER TABLE Adresy ADD CreationDate DateTime2(4) NOT NULL DEFAULT (GETDATE());
ALTER TABLE Adresy ADD ModificationDate DateTime2(4) NOT NULL DEFAULT (GETDATE());
ALTER TABLE Adresy DROP COLUMN Guid;
GO
ALTER TABLE Adresy ADD Guid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID());
GO
UPDATE Adresy SET ModificationDate = CreationDate;

UPDATE Adresy SET DomyslnyAdresFaktury = 0 WHERE DomyslnyAdresFaktury IS NULL;
UPDATE Adresy SET DomyslnyAdresWysylki = 0 WHERE DomyslnyAdresWysylki IS NULL;
UPDATE Adresy SET DomyslnyAdresFaktury = 1 WHERE DomyslnyAdresFaktury = 0 AND DomyslnyAdresWysylki = 0;
UPDATE Adresy SET Usuniety = 0 WHERE Usuniety IS NULL;
UPDATE Adresy SET Usuniety = 1 WHERE Synchronizacja = 4;

ALTER TABLE Adresy ADD CONSTRAINT DF_Address_IsDeleted DEFAULT (0) FOR Usuniety;
GO
ALTER TABLE Adresy ALTER COLUMN Usuniety BIT NOT NULL;
ALTER TABLE Adresy ALTER COLUMN DomyslnyAdresFaktury BIT NOT NULL;
ALTER TABLE Adresy ALTER COLUMN DomyslnyAdresWysylki BIT NOT NULL;
GO
WITH CustomersWithBadAddresses AS
(
	SELECT Kontrahent FROM Adresy  WHERE Usuniety=0 AND DomyslnyAdresFaktury = 1 GROUP BY Kontrahent HAVING count(*) > 1
)
UPDATE Adresy SET Usuniety = 1
WHERE Kontrahent in (SELECT Kontrahent FROM CustomersWithBadAddresses)
 AND Id NOT IN (SELECT MIN(Id) FROM Adresy WHERE Usuniety=0 AND DomyslnyAdresFaktury = 1 AND Kontrahent IN (SELECT Kontrahent FROM CustomersWithBadAddresses) group by Kontrahent);
GO

WITH CustomersWithBadAddresses AS
(
	SELECT Kontrahent FROM Adresy  WHERE Usuniety=0 AND DomyslnyAdresWysylki = 1 GROUP BY Kontrahent HAVING count(*) > 1
)

UPDATE Adresy SET DomyslnyAdresWysylki = 0
WHERE
 Kontrahent in (SELECT Kontrahent FROM CustomersWithBadAddresses)
 AND Id NOT IN (SELECT MIN(Id) FROM Adresy WHERE Usuniety=0 AND DomyslnyAdresWysylki = 1 AND Kontrahent IN (SELECT Kontrahent FROM CustomersWithBadAddresses) group by Kontrahent);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_UNIQUE_Customer_IsDefaultInvoice] ON [dbo].[Adresy]
(
	[Kontrahent] ASC,
	[DomyslnyAdresFaktury] ASC
)
WHERE ([Usuniety]=(0) AND [Synchronizacja]<>(4) AND [DomyslnyAdresFaktury]=(1));
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_UNIQUE_Customer_IsDefaultDelivery] ON [dbo].[Adresy]
(
	[Kontrahent] ASC,
	[DomyslnyAdresWysylki] ASC
)
WHERE ([Usuniety]=(0) AND [Synchronizacja]<>(4) AND [DomyslnyAdresWysylki]=(1));
GO





 