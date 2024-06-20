Use Master
GO
Create Database Pessoas
GO
USE [Pessoas]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pessoa](
	[IdPessoa] [int] IDENTITY(1,1) NOT NULL,
	[NomePessoa] [varchar](200) NOT NULL,
	[IdadePessoa] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPessoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Dependentes]  WITH CHECK ADD FOREIGN KEY([IdPessoa])
REFERENCES [dbo].[Pessoa] ([IdPessoa])
GO
CREATE TABLE [dbo].[Dependentes](
	[IdDependente] [int] IDENTITY(1,1) NOT NULL,
	[NomeDependente] [varchar](200) NOT NULL,
	[IdadeDependente] [varchar](200) NOT NULL,
	[IdPessoa] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDependente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Atualiza_Dependentes]
    @IdDependente INT,
    @NomeDependente VARCHAR(200),
    @IdadeDependente date,
	@IdPessoa int
AS
BEGIN
    UPDATE Pessoas.dbo.Dependentes
    SET NomeDependente = @NomeDependente, IdadeDependente = @IdadeDependente
    WHERE IdDependente = @IdDependente
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Atualiza_Pessoa]
    @IdPessoa INT
AS
BEGIN
	Exec dbo.SP_Deleta_Pessoa @IdPessoa
END	
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Busca_Pessoa]
(
    @NomePessoa VARCHAR(200),
    @IdadePessoa date
)
AS
BEGIN
	Drop Table IF EXISTS #Temp 
	CREATE Table #Temp 
	(
		 IdPessoa			int	
		,NomePessoa			varchar(200)
		,IdadePessoa		varchar(200)
		,IdDependente		varchar(200)
		,NomeDependente		varchar(200)
		,IdadeDependente	varchar(200)
		,IdPessoaFK			int
	)
	INSERT INTO #Temp (IdPessoa,NomePessoa,IdadePessoa,IdDependente,NomeDependente,IdadeDependente,IdPessoaFK)
		SELECT 
			* 
		FROM 
			Pessoas.dbo.Pessoa AS A
		Inner Join Pessoas.dbo.Dependentes AS B ON B.IdPessoa = A.IdPessoa
		Where
			NomePessoa = @NomePessoa AND
			IdadePessoa = @IdadePessoa
	INSERT INTO #Temp 
		Select 
			*, '','','','' from Pessoas.dbo.Pessoa AS A
		Where 
			NOT EXISTS (Select * from #Temp AS B where A.IdPessoa = B.IdPessoa) AND
			NomePessoa = @NomePessoa AND
			IdadePessoa = @IdadePessoa 
	Select * from #Temp
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Busca_Pessoa_Por_Id]
(
	@IdPessoa int
)
AS
BEGIN
	IF NOT EXISTS (
		Select 
			* 
		From
			Pessoas.dbo.Dependentes 
		Where
			IdPessoa = @IdPessoa
	)
	BEGIN
		Select 
			 IdPessoa
			,NomePessoa
			,Format(cast(IdadePessoa as datetime), 'yyyy-MM-dd') AS IdadePessoa
			,'' AS IdDependente
			,'' AS NomeDependente
			,''	AS IdadeDependente
			,''	AS IdPessoa
		From
			Pessoas.dbo.Pessoa
		Where
			IdPessoa = @IdPessoa
	END
	ELSE
	BEGIN
		Select 
			 A.IdPessoa
			,NomePessoa
			,Format(cast(IdadePessoa as datetime), 'yyyy-MM-dd') AS IdadePessoa
			,IdDependente
			,NomeDependente
			,Format(cast(IdadeDependente as datetime), 'yyyy-MM-dd') AS IdadeDependente
			,B.IdPessoa
		From
			Pessoas.dbo.Pessoa AS A
		INNER JOIN
			Pessoas.dbo.Dependentes AS B ON B.IdPessoa = A.IdPessoa
		Where
			A.IdPessoa = @IdPessoa
	END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BuscaGeral_Pessoa]
AS
BEGIN
	Drop Table IF EXISTS #Temp 
	CREATE Table #Temp 
	(
		 IdPessoa			int	
		,NomePessoa			varchar(200)
		,IdadePessoa		date
		,IdDependente		int
		,NomeDependente		varchar(200)
		,IdadeDependente	date
		,IdPessoaFK			int
	)
	INSERT INTO #Temp (IdPessoa,NomePessoa,IdadePessoa,IdDependente,NomeDependente,IdadeDependente,IdPessoaFK)
    SELECT 
		* 
    FROM 
		Pessoas.dbo.Pessoa AS A
	Inner Join Pessoas.dbo.Dependentes AS B ON B.IdPessoa = A.IdPessoa
	INSERT INTO #Temp 
		Select *, '','','','' from Pessoas.dbo.Pessoa AS A
		Where NOT EXISTS (Select * from #Temp AS B where A.IdPessoa = B.IdPessoa)
	Select * from #Temp
	
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Deleta_Dependentes]
    @IdPessoa INT
AS
BEGIN
    Delete From	
		Pessoas.dbo.Dependentes
    WHERE 
		IdPessoa = @IdPessoa
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Deleta_Pessoa]
(
	@IdPessoa int
)
AS
BEGIN
	Delete From
		Pessoas.dbo.Dependentes
	Where
		IdPessoa = @IdPessoa
	Delete From 
		Pessoas.dbo.Pessoa 
	Where
		IdPessoa = @IdPessoa
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_Inseri_Dependente]
    @IdPessoa INT,
    @NomeDependente VARCHAR(200),
    @IdadeDependente VARCHAR(200)
AS
BEGIN
    INSERT INTO Pessoas.dbo.Dependentes (IdPessoa, NomeDependente, IdadeDependente)
    VALUES (@IdPessoa, @NomeDependente, @IdadeDependente)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Inseri_Pessoa]
(
    @NomePessoa VARCHAR(200),
    @IdadePessoa VARCHAR(200),
    @IdPessoa INT OUTPUT  -- Parâmetro de saída para retornar o IdPessoa inserido
)
AS
BEGIN
    INSERT INTO Pessoas.dbo.Pessoa (NomePessoa, IdadePessoa)
    VALUES (@NomePessoa, @IdadePessoa);

    SET @IdPessoa = SCOPE_IDENTITY();
END
GO