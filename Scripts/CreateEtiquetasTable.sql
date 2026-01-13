-- Script para crear la tabla Etiquetas en SQL Server
-- Ejecutar este script en la base de datos configurada en appsettings.json

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Etiquetas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Etiquetas] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Nombre] NVARCHAR(255) NOT NULL,
        [Width] INT NOT NULL,
        [Height] INT NOT NULL,
        [Objects] NVARCHAR(MAX) NULL, -- JSON del canvas
        [FechaGuardado] DATETIME NOT NULL DEFAULT GETDATE()
    );

    -- Crear índice en Nombre para búsquedas rápidas
    CREATE INDEX IX_Etiquetas_Nombre ON [dbo].[Etiquetas]([Nombre]);
    
    -- Crear índice en FechaGuardado para ordenar por fecha
    CREATE INDEX IX_Etiquetas_FechaGuardado ON [dbo].[Etiquetas]([FechaGuardado] DESC);

    PRINT 'Tabla Etiquetas creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Etiquetas ya existe';
END
GO
