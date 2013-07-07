namespace ORMPerformanceTest.Tests.Helpers
{
    public static class Const
    {
        #region [Const]

        public const string DBCreateScript = @"--use master;
--CREATE DATABASE ORMTest;
--GO
--USE ORMTest;

if  OBJECT_ID('Home') is not null
drop table Home;

if  OBJECT_ID('Province') is not null
drop table Province;

CREATE TABLE Province(
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(250),
	Code INT
);

ALTER TABLE Province ADD PRIMARY KEY (Id);


CREATE TABLE Home(
	Id INT IDENTITY(1,1) NOT NULL ,
	Surface INT,
	Price DECIMAL(18,2),
	BuildYear INT,
	City NVARCHAR(250),
	Description NVARCHAR(max),
	ProvinceId INT,
    AddTime datetime
);

ALTER TABLE Home ADD PRIMARY KEY (Id);
ALTER TABLE Home ADD FOREIGN KEY (ProvinceId) REFERENCES Province(Id)";
        #endregion [Const]
    }
}