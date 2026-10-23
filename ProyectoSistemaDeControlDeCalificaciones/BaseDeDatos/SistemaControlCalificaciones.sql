CREATE DATABASE ControlCalificaciones;
GO

USE ControlCalificaciones;
GO

-- TABLA DE ESTUDIANTES
CREATE TABLE Estudiantes (
    IdEstudiante INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Carnet VARCHAR(20) NOT NULL UNIQUE
);
GO

-- TABLA DE MATERIAS
CREATE TABLE Materias (
    IdMateria INT IDENTITY(1,1) PRIMARY KEY,
    NombreMateria VARCHAR(100) NOT NULL
);
GO

-- TABLA DE CALIFICACIONES
CREATE TABLE Calificaciones (
    IdCalificacion INT IDENTITY(1,1) PRIMARY KEY,
    IdEstudiante INT NOT NULL,
    IdMateria INT NOT NULL,
    Nota DECIMAL(5,2) NOT NULL,

    CONSTRAINT CK_Calificaciones_Nota
        CHECK (Nota >= 0 AND Nota <= 10),

    CONSTRAINT FK_Calificaciones_Estudiantes
        FOREIGN KEY (IdEstudiante)
        REFERENCES Estudiantes(IdEstudiante),

    CONSTRAINT FK_Calificaciones_Materias
        FOREIGN KEY (IdMateria)
        REFERENCES Materias(IdMateria),

    CONSTRAINT UQ_Calificacion_Estudiante_Materia
        UNIQUE (IdEstudiante, IdMateria)
);
GO

--- AQUIIII COMENZAMOS HACIENDO LAS PRUEBAS INSERTANDO LOS DATOS 
--- Y HACIENDO LAS CONSULTAS

-- DATOS DE PRUEBA DE ESTUDIANTES
INSERT INTO Estudiantes (Nombre, Apellido, Carnet)
VALUES
('Diego', 'Umaña Rivas', '260327'),
('Adán', 'Cushco Shunico', '260396'),
('Javier', 'Echeverría Juárez', '260512'),
('Yaquelin', 'Echeverría Vásquez', '260862'),
('Carlos', 'Martínez', '260410'),
('Ana', 'López', '260455'),
('Luis', 'Hernández', '260501'),
('Sofía', 'Ramírez', '260530');
GO

-- DATOS DE PRUEBA DE MATERIA
INSERT INTO Materias (NombreMateria)
VALUES ('Programación I');
GO

-- DATOS DE PRUEBA DE CALIFICACIONES
INSERT INTO Calificaciones (IdEstudiante, IdMateria, Nota)
VALUES
(1, 1, 8.50),
(2, 1, 6.75),
(3, 1, 9.25),
(4, 1, 7.50),
(5, 1, 5.25),
(6, 1, 9.00),
(7, 1, 4.50),
(8, 1, 7.00);
GO

-- CONSULTA GENERAL
-- Usamos join por que los datos estan separado y
-- lo queremos hacer es unir las tablas relacionadas
-- Para poder mostrar la informacion completa
SELECT
    e.IdEstudiante,
    e.Nombre,
    e.Apellido,
    e.Carnet,
    m.NombreMateria,
    c.Nota
FROM Calificaciones c
INNER JOIN Estudiantes e
    ON c.IdEstudiante = e.IdEstudiante
INNER JOIN Materias m
    ON c.IdMateria = m.IdMateria
ORDER BY e.IdEstudiante;
GO

-- RESUMEN DEL GRUPO, MUESTRA EL PROMEDIO, NOTA MAYOR, NOTA MENOR,
-- Y CUANTOS HAN APROBADOS Y REPROBADOS
-- Se considera aprobado con nota igual o mayor a 6.00.
SELECT
    ROUND(AVG(Nota), 2) AS PromedioGeneral,
    MAX(Nota) AS NotaMayor,
    MIN(Nota) AS NotaMenor,
    SUM(CASE WHEN Nota >= 6.00 THEN 1 ELSE 0 END) AS Aprobados,
    SUM(CASE WHEN Nota < 6.00 THEN 1 ELSE 0 END) AS Reprobados
FROM Calificaciones;
GO

-- ESTO HACE QUE SE MUESTRE EL ESTADO DE CADA ESTUDIANTE
SELECT
    e.Carnet,
    e.Nombre,
    e.Apellido,
    c.Nota,
    CASE
        WHEN c.Nota >= 6.00 THEN 'Aprobado'
        ELSE 'Reprobado'
    END AS Estado
FROM Calificaciones c
INNER JOIN Estudiantes e
    ON c.IdEstudiante = e.IdEstudiante
ORDER BY e.Apellido, e.Nombre;
GO
