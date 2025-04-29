-- ===========================
-- ELIMINAR LAS TABLAS SI EXISTEN (DROP)
-- ===========================
DROP TABLE IF EXISTS PAGO;
DROP TABLE IF EXISTS BOLETO;
DROP TABLE IF EXISTS ASIENTO;
DROP TABLE IF EXISTS VERSION;
DROP TABLE IF EXISTS USUARIO;
DROP TABLE IF EXISTS REPARTO;
DROP TABLE IF EXISTS PELICULA_GENERO;
DROP TABLE IF EXISTS PELICULA_COMPANIA;
DROP TABLE IF EXISTS PELICULA_DIRECTOR;
DROP TABLE IF EXISTS PELICULA;
DROP TABLE IF EXISTS SALA_TIPO;
DROP TABLE IF EXISTS TIPO_SALA;
DROP TABLE IF EXISTS SALA;
DROP TABLE IF EXISTS CINE;
DROP TABLE IF EXISTS DIRECTOR;
DROP TABLE IF EXISTS ACTOR;
DROP TABLE IF EXISTS GENERO;
DROP TABLE IF EXISTS COMPANIA;
DROP TABLE IF EXISTS HORARIO_PELICULA;

-- ===========================
-- CREACIÓN DE TABLAS
-- ===========================

-- Tabla de Cines
CREATE TABLE CINE (
    CINE_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    UBICACION NVARCHAR(255) NOT NULL
);

-- Tabla de Salas
CREATE TABLE SALA (
    SALA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    CINE_ID INT NOT NULL,
    CONSTRAINT FK_SALA_CINE FOREIGN KEY (CINE_ID) REFERENCES CINE(CINE_ID)
);

-- Tabla de Tipos de Sala
CREATE TABLE TIPO_SALA (
    TIPO_SALA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(50) NOT NULL UNIQUE
);

-- Tabla intermedia para la relación muchos a muchos entre SALA y TIPO_SALA
CREATE TABLE SALA_TIPO (
    SALA_ID INT NOT NULL,
    TIPO_SALA_ID INT NOT NULL,
    PRIMARY KEY (SALA_ID, TIPO_SALA_ID),
    CONSTRAINT FK_SALA_TIPO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID),
    CONSTRAINT FK_SALA_TIPO_TIPO FOREIGN KEY (TIPO_SALA_ID) REFERENCES TIPO_SALA(TIPO_SALA_ID)
);

-- Tabla de Actores
CREATE TABLE ACTOR (
    ACTOR_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX) NULL,
    IMG_PERFIL NVARCHAR(255) NULL,
    FECHA_NACIMIENTO DATETIME NULL
);

-- Tabla de Directores
CREATE TABLE DIRECTOR (
    DIRECTOR_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX),
    IMG_PERFIL NVARCHAR(255),
    FECHA_NACIMIENTO DATETIME
);

-- Tabla de Géneros
CREATE TABLE GENERO (
    GENERO_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL
);

-- Tabla de Compañías
CREATE TABLE COMPANIA (
    COMPANIA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    SEDE NVARCHAR(255) NULL,
    WEB NVARCHAR(200) NULL,
    IMG_LOGO NVARCHAR(MAX) NULL,
    PAIS_ORIGEN NVARCHAR(10)
);

-- Tabla de Películas
CREATE TABLE PELICULA (
    PELICULA_ID INT PRIMARY KEY,
    TITULO NVARCHAR(255) NOT NULL,
    FECHA_LANZAMIENTO DATE,
    DURACION_MINUTOS INT,
    TITULO_ETIQUETA NVARCHAR(255),
    SINOPSIS NVARCHAR(MAX),
    IMG_FONDO NVARCHAR(255),
    IMG_POSTER NVARCHAR(255)
);

-- Tabla de Reparto (solo actores)
CREATE TABLE REPARTO (
    REPARTO_ID INT PRIMARY KEY,
    PELICULA_ID INT NOT NULL,
    ACTOR_ID INT NOT NULL,
    NOMBRE NVARCHAR(255) NULL,
    PERSONAJE NVARCHAR(255) NULL,
    IMG_PERFIL NVARCHAR(255) NULL,
    CONSTRAINT FK_REPARTO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_REPARTO_ACTOR FOREIGN KEY (ACTOR_ID) REFERENCES ACTOR(ACTOR_ID)
);

-- Tabla intermedia Películas - Géneros
CREATE TABLE PELICULA_GENERO (
    PELICULA_ID INT NOT NULL,
    GENERO_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, GENERO_ID),
    CONSTRAINT FK_PELICULA_GENERO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_GENERO_GENERO FOREIGN KEY (GENERO_ID) REFERENCES GENERO(GENERO_ID)
);

-- Tabla intermedia Películas - Compañías
CREATE TABLE PELICULA_COMPANIA (
    PELICULA_ID INT NOT NULL,
    COMPANIA_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, COMPANIA_ID),
    CONSTRAINT FK_PELICULA_COMPANIA_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_COMPANIA_COMPANIA FOREIGN KEY (COMPANIA_ID) REFERENCES COMPANIA(COMPANIA_ID)
);

-- Tabla intermedia Películas - Directores
CREATE TABLE PELICULA_DIRECTOR (
    PELICULA_ID INT NOT NULL,
    DIRECTOR_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, DIRECTOR_ID),
    CONSTRAINT FK_PELICULA_DIRECTOR_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_DIRECTOR_DIRECTOR FOREIGN KEY (DIRECTOR_ID) REFERENCES DIRECTOR(DIRECTOR_ID)
);

-- Tabla de Usuarios
CREATE TABLE USUARIO (
    USUARIO_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    EMAIL NVARCHAR(150) UNIQUE NOT NULL,
	IMAGEN NVARCHAR(150) NULL,
	SALT NVARCHAR(50) NOT NULL,
    PASS VARBINARY(MAX) NOT NULL,
    FECHA_CREACION DATETIME NOT NULL
);

-- Tabla de Versiones
CREATE TABLE VERSION (
    VERSION_ID INT PRIMARY KEY,
    IDIOMA NVARCHAR(50) NOT NULL,
);

-- Tabla de Horarios de Películas
CREATE TABLE HORARIO_PELICULA (
    HORARIO_ID INT PRIMARY KEY,
    PELICULA_ID INT NOT NULL,
    SALA_ID INT NOT NULL,
    VERSION_ID INT NOT NULL,
    HORA_FUNCION DATETIME NOT NULL,
    ASIENTOS_DISPONIBLES INT NOT NULL,
	ESTADO CHAR(1) NOT NULL,
    CONSTRAINT FK_HORARIO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_HORARIO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID),
    CONSTRAINT FK_HORARIO_VERSION FOREIGN KEY (VERSION_ID) REFERENCES VERSION(VERSION_ID)
);

-- Tabla de Asientos
CREATE TABLE ASIENTO (
    ASIENTO_ID INT PRIMARY KEY,
    SALA_ID INT NOT NULL,
	HORARIO_ID INT NOT NULL,
    NUMERO NVARCHAR(10) NOT NULL,
    FILA NVARCHAR(5) NOT NULL,
    DISPONIBLE BIT DEFAULT 1,
    CONSTRAINT FK_ASIENTO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID),
    CONSTRAINT FK_ASIENTO_HORARIO_PELICULA FOREIGN KEY (HORARIO_ID) REFERENCES HORARIO_PELICULA(HORARIO_ID)
);

-- Tabla de Boletos
CREATE TABLE BOLETO (
    BOLETO_ID INT PRIMARY KEY,
    USUARIO_ID INT NOT NULL,
    HORARIO_ID INT NOT NULL,
    ASIENTO_ID INT NOT NULL,
    FECHA_COMPRA DATETIME DEFAULT GETDATE(),
    ESTADO NVARCHAR(20) DEFAULT 'reservado',
    CONSTRAINT FK_BOLETO_ASIENTO FOREIGN KEY (ASIENTO_ID) REFERENCES ASIENTO(ASIENTO_ID)
);

-- Tabla de Pagos
CREATE TABLE PAGO (
    PAGO_ID INT PRIMARY KEY,
    BOLETO_ID INT NOT NULL,
    MONTO DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_PAGO_BOLETO FOREIGN KEY (BOLETO_ID) REFERENCES BOLETO(BOLETO_ID)
);

-- Insertar Cine
INSERT INTO CINE (CINE_ID, NOMBRE, UBICACION)
VALUES (1, 'Cine Central', 'Av. Principal 123');

-- Insertar 6 salas manualmente
INSERT INTO SALA (SALA_ID, NOMBRE, CINE_ID)
VALUES 
    (1, 'Sala 1', 1),
    (2, 'Sala 2', 1),
    (3, 'Sala 3', 1),
    (4, 'Sala 4', 1),
    (5, 'Sala 5', 1),
    (6, 'Sala 6', 1);

-- Insertar los 120 asientos por cada sala

--DECLARE @SALA_ID INT;
--DECLARE @FILA CHAR(1);
--DECLARE @NUMERO INT;
--DECLARE @ASIENTO_ID INT = 1; -- Control manual del ID de los asientos

--DECLARE CURSOR_SALAS CURSOR FOR 
--SELECT SALA_ID FROM SALA;

--OPEN CURSOR_SALAS;
--FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;

--WHILE @@FETCH_STATUS = 0
--BEGIN
--    -- Recorrer filas (A - J)
--    DECLARE @FILA_INDEX INT = 1;
--    WHILE @FILA_INDEX <= 10
--    BEGIN
--        SET @FILA = CHAR(64 + @FILA_INDEX); -- Convierte 1 -> 'A', 2 -> 'B', ..., 10 -> 'J'
--        SET @NUMERO = 1;
        
--        -- Recorrer asientos (1 - 12)
--        WHILE @NUMERO <= 12
--        BEGIN
--            INSERT INTO ASIENTO (ASIENTO_ID, SALA_ID, NUMERO, FILA, DISPONIBLE) 
--            VALUES (@ASIENTO_ID, @SALA_ID, CAST(@NUMERO AS NVARCHAR(10)), @FILA, 1);
            
--            SET @NUMERO = @NUMERO + 1;
--            SET @ASIENTO_ID = @ASIENTO_ID + 1; -- Incrementar manualmente el ID
--        END
        
--        SET @FILA_INDEX = @FILA_INDEX + 1;
--    END
    
--    FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;
--END

--CLOSE CURSOR_SALAS;
--DEALLOCATE CURSOR_SALAS;

-- ====================================
-- Insertar Versiones
-- ====================================

INSERT INTO VERSION (VERSION_ID, IDIOMA)
VALUES (1, 'Español'),
       (2, 'Inglés');

-- ====================================
-- Insertar peliculas
-- ====================================
SET DATEFORMAT dmy;
INSERT INTO PELICULA (PELICULA_ID, SINOPSIS, FECHA_LANZAMIENTO, DURACION_MINUTOS, TITULO_ETIQUETA, TITULO, IMG_FONDO, IMG_POSTER)
VALUES
(950396, 'Dos agentes de élite son secretamente asignados a torres de vigilancia en los lados opuestos de un vasto desfiladero, para proteger al mundo de un misterioso mal que acecha en su interior. Se unen en la distancia, pero han de mantenerse alerta para defenderse del enemigo invisible. Cuando se les revela una amenaza fatal para la humanidad, deben trabajar juntos y poner a prueba su fuerza física y mental para mantener el secreto del desfiladero antes de que sea demasiado tarde.', '2025-02-13',127, 'Nadie entra. Nada sale.', 'El abismo secreto', 
'https://image.tmdb.org/t/p/w533_and_h300_bestv2/9nhjGaFLKtddDPtPaX5EmKqsWdH.jpg',
'https://image.tmdb.org/t/p/w300_and_h450_bestv2/3s0jkMh0YUhIeIeioH3kt2X4st4.jpg'),
(762509,'Rafiki debe transmitir la leyenda de Mufasa a la joven cachorro de león Kiara, hija de Simba y Nala, y con Timón y Pumba prestando su estilo característico. Mufasa, un cachorro huérfano, perdido y solo, conoce a un simpático león llamado Taka, heredero de un linaje real. Este encuentro casual pone en marcha un viaje de un extraordinario grupo de inadaptados que buscan su destino.','2024-12-18', 118,'','Mufasa: El rey león',
'https://image.tmdb.org/t/p/w533_and_h300_bestv2/jIgM7YNVft0YGeXsqrh3oG5TWLx.jpg',
'https://image.tmdb.org/t/p/w300_and_h450_bestv2/dmw74cWIEKaEgl5Dv3kUTcCob6D.jpg'),
(1126166,'En este claustrofóbico thriller, un piloto (Mark Wahlberg) transporta en su avioneta a una teniente general (Michelle Dockery) que custodia a un testigo (Topher Grace) que va a testificar en un juicio contra la mafia. A medida que atraviesan las montañas de Alaska, las tensiones se disparan, ya que no todo el mundo a bordo es quien parece ser. Y a 3.000 metros de altura no hay escapatoria posible.','2025-01-22',91,'','Amenaza en el aire',
'https://image.tmdb.org/t/p/w533_and_h300_bestv2/ek8CJRZchT9YIB4p7ktEjPXuCIi.jpg',
'https://image.tmdb.org/t/p/w300_and_h450_bestv2/8T6nkYb4W8BIeafmFffyfsRciTL.jpg'),
(939243,'Sonic, Knuckles y Tails se reúnen para enfrentarse a un nuevo y poderoso adversario, Shadow, un misterioso villano cuyos poderes no se parecen a nada de lo que nuestros héroes han conocido hasta ahora. Con sus facultades superadas en todos los sentidos, el Equipo Sonic tendrá que establecer una insólita alianza con la esperanza de detener a Shadow y proteger el planeta.','2024-12-19',110,'','Sonic 3: La película',
'https://image.tmdb.org/t/p/w533_and_h300_bestv2/ggDVF2DhqIQjJI0zUqsWMS2WTcF.jpg',
'https://image.tmdb.org/t/p/w300_and_h450_bestv2/3aDWCRXLYOCuxjrjiPfLd79tcI6.jpg'),
(1241982,'Tras recibir una inesperada llamada de sus antepasados, Vaiana debe viajar a los lejanos mares de Oceanía y adentrarse en peligrosas aguas perdidas para vivir una aventura sin precedentes.','2024-11-21',99,'','Vaiana 2',
'https://image.tmdb.org/t/p/w533_and_h300_bestv2/7rgHEYy23gAmxTyxivGXBTBikA1.jpg',
'https://image.tmdb.org/t/p/w300_and_h450_bestv2/b1WsCRfomw7tRi12NuseKsAJxYK.jpg');

-- ====================================
-- Insertar companias
-- ====================================

INSERT INTO COMPANIA (COMPANIA_ID, NOMBRE, SEDE, WEB, IMG_LOGO, PAIS_ORIGEN) 
VALUES 
(694, 'StudioCanal', 'Paris', 'http://www.studiocanal.fr', '/5LEHONGkZBIoWvp1ygHOF8iyi1M.png', 'FR' ),
(238886, 'Marmalade Pictures', 'London, England', NULL, NULL, 'GB'),
(73025, 'Kinoshita Group', NULL, 'https://www.kinoshita-group.co.jp', '/zYb2OfWjKvNlngpq4LemY9JVC51.png', 'JP'),
(11341, 'Stage 6 Films', 'Culver City, California', 'https://www.stage6films.com', '/xytTBODEy3p20ksHL4Ftxr483Iv.png', 'US');


-- ====================================
-- Insertar actores
-- ====================================
INSERT INTO ACTOR (BIOGRAFIA ,FECHA_NACIMIENTO,ACTOR_ID, NOMBRE,IMG_PERFIL)
VALUES
('',1980-10-14,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg'),
('',1963-11-10,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg'),
('',1971-12-01,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg'),
('',2002-01-18,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg'),
('',2001-04-28,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg'),
('Actor y director español que ha alcanzado gran fama dentro del panorama cinematográfico internacional. Nació en Málaga e inició su carrera como actor en el seno una compañía de teatro independiente. Tras fijar su residencia en Madrid, comenzó a trabajar en el cine como actor secundario. En 1982 rodó Laberinto de pasiones, su primera película con Pedro Almodóvar, director gracias al que alcanzó gran popularidad a partir de su intervención en filmes como La ley del deseo (1986), Mujeres al borde de un ataque de nervios (1987) y Átame (1989). También colaboró en producciones de otros realizadores españoles, como Los zancos (1984), de Carlos Saura, Bajarse al moro (1988), de Fernando Colomo, y Si te dicen que caí (1989), de Vicente Aranda. Con Los reyes del mambo tocan canciones de amor (1992, de Arne Glimcher), basada en la novela del mismo título de Oscar Hijuelos, Banderas comenzó a actuar en producciones extranjeras. Después de divorciarse de su primera mujer, Ana Leza, y de fijar su residencia en Estados Unidos, contrajo matrimonio con la popular actriz estadounidense Melanie Griffith. Entre su filmografía destacan algunos títulos muy conocidos que lo han lanzado al estrellato, como Philadelphia (1993, de Jonathan Demme); La casa de los espíritus (1993, de Bille August), basado en la novela de la escritora chilena Isabel Allende; Entrevista con el vampiro (1994, de Neil Jordan); Two much (1995, de Fernando Trueba); Evita (1996, de Alan Parker), con Madonna; La máscara del Zorro (1998, de Martin Campbell), en la que da vida al célebre mito literario y del cine de acción, y Frida (2002, de Julie Taymor), donde interpreta al artista mexicano David Alfaro Siqueiros. También ha prestado su voz al personaje del gato con botas en la popular película de dibujos animados Shrek 2 (2004). En 1999 estrenó su primera película como director, Locos en Albama, protagonizada por su mujer. Tras sufrir un ataque al corazón el 26 de enero de 2017 el actor asegura estar ya bien y vaticina que lo mejor de su carrera “está por venír”.',1960-08-10,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg'),
('',1974-01-30,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg'),
('',1950-02-22,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg'),
('Nació en Lincoln, hijo de Doreen "Dee" Findlay, una escultora, y Roy Laverick Broadbent, un artista, escultor y diseñador de interiores.1?2? Los padres de Broadbent fueron actores aficionados y fundaron el grupo teatral Holton Players.3? Tuvo una hermana melliza que murió al nacer. Broadbent estudió en la Leighton Park School, una escuela cuáquera ubicada en Reading.4? Posteriormente estudió en la London Academy of Music and Dramatic Art.',1949-05-24,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg'),
('Carla Tous (España, 2001) es una actriz española .',2001-01-01,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg'),
('',1976-01-19,65451,'Oliver Maltman','/wZgJ4BhseHlGOakiSMLCDUE07ic.jpg'),
('',1984-05-20,54811,'Joel Fry','/4nEKEWJpaTHncCTv6zeP98V0qGI.jpg'),
('',1970-03-24,1119,'Robbie Gee','/mzzfbA0e4DWgxvIYjaryMolkiqm.jpg'),
('',1963-10-31,145997,'Sanjeev Bhaskar','/gPFoGWj5VPm4IZVn9sMdBicVh2E.jpg'),
('',1956-01-09,11356,'Imelda Staunton','/tABRYOHUQeaUCAqrelJV5ZHjl1W.jpg'),
('',1966-02-24,18025,'Ben Miller','/ij5nhtBWhCaOl4IEn8XhOYAGqB1.jpg'),
('',1972-10-30,47730,'Jessica Hynes','/eyJ9aC1FXyPAI5xose0HqA26bnm.jpg'),
('',NULL,3037851,'Ella Dacres','/x9yadKWs3mWLXEZ6gcgvgCzhbr1.jpg'),
('',NULL,2329015,'Aloreia Spencer',NULL),
('',1977-01-01,110076,'Nicholas Burns','/x8ETzUUeTrmBRuVdvPEaZ23Focf.jpg'),
('',1995-08-12,2825348,'Ashleigh Reynolds','/jqZs6a76NM6Yks7fGy5azmaYbZD.jpg'),
('',1981-04-26,150072,'Amit Shah','/s6zx8CumxTGq0kULxp6H6nc1DDA.jpg'),
('',NULL,3250136,'Ella Bruccoleri','/xVqbUOELKqxhEdqAz5IXhFYp7Uy.jpg'),
('',1971-04-15,1456143,'Carlos Carlín','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg'),
('',1973-04-02,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg'),
('',1991-02-05,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg'),
('',1960-09-09,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg');

--INSERT INTO REPARTO (ID_PELICULA, ID_PERSONA,NOMBRE ,IMG_PERFIL,PERSONAJE)
--VALUES
--(516729,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg','Paddington Brown (voice)'),
--(516729,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg','Henry Brown'),
--(516729,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg','Mary Brown'),
--(516729,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg','Jonathan Brown'),
--(516729,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg','Judy Brown'),
--(516729,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg','Hunter Cabot'),
--(516729,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg','Reverend Mother'),
--(516729,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg','Mrs. Bird'),
--(516729,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg','Samuel Gruber'),
--(516729,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg','Gina Cabot'),
--(516729,39459,'Hayley Atwell','/iQyJYuVWOA6tUxQEHt6dSuz1PoE.jpg','Madison'),
--(516729,65451,'Oliver Maltman','/wZgJ4BhseHlGOakiSMLCDUE07ic.jpg','Photo Booth (voice)'),
--(516729,54811,'Joel Fry','/4nEKEWJpaTHncCTv6zeP98V0qGI.jpg','Joe the Postman'),
--(516729,1119,'Robbie Gee','/mzzfbA0e4DWgxvIYjaryMolkiqm.jpg','Mr. Barnes'),
--(516729,145997,'Sanjeev Bhaskar','/gPFoGWj5VPm4IZVn9sMdBicVh2E.jpg','Dr. Jafri'),
--(516729,11356,'Imelda Staunton','/tABRYOHUQeaUCAqrelJV5ZHjl1W.jpg','Aunt Lucy (voice)'),
--(516729,18025,'Ben Miller','/ij5nhtBWhCaOl4IEn8XhOYAGqB1.jpg','Colonel Lancaster'),
--(516729,47730,'Jessica Hynes','/eyJ9aC1FXyPAI5xose0HqA26bnm.jpg','Miss Kitts'),
--(516729,3037851,'Ella Dacres','/x9yadKWs3mWLXEZ6gcgvgCzhbr1.jpg','Student Rep'),
--(516729,2329015,'Aloreia Spencer',NULL,'Madison’s Assistant'),
--(516729,110076,'Nicholas Burns','/x8ETzUUeTrmBRuVdvPEaZ23Focf.jpg','Mr. Wilson'),
--(516729,2825348,'Ashleigh Reynolds','/jqZs6a76NM6Yks7fGy5azmaYbZD.jpg','Mr. Smith'),
--(516729,150072,'Amit Shah','/s6zx8CumxTGq0kULxp6H6nc1DDA.jpg','Zayden'),
--(516729,3250136,'Ella Bruccoleri','/xVqbUOELKqxhEdqAz5IXhFYp7Uy.jpg','Rosita'),
--(516729,1456143,'Carlos Carlín','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg','Passport Control Officer'),
--(516729,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg','Barry the Air Steward'),
--(516729,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg','Elderly Resident Bear 3 (voice)'),
--(516729,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg','Phoenix Buchanan');

-- ====================================
-- Insertar Horarios Pelicula
-- ====================================

--insert into HORARIO_PELICULA
--(HORARIO_ID, PELICULA_ID, SALA_ID, HORARIO_ID, VERSION_ID, HORA_FUNCION, ASIENTOS_DISPONIBLES, ESTADO)
--values
--(123, 762509, 1, 1,  '2025-03-03 18:30:00', 20, 'A')

--CREATE VIEW V_FACTURA_BOLETO
--AS
--SELECT 
--	CAST(ROW_NUMBER() OVER (ORDER BY USUARIO.USUARIO_ID) as int) AS ID,
--	USUARIO.USUARIO_ID AS USUARIO_ID,
--    PELICULA.TITULO AS PELICULA,
--    CINE.NOMBRE AS CINE,
--    HORARIO_PELICULA.HORA_FUNCION AS HORA_FUNCION,
--    COUNT(BOLETO.BOLETO_ID) AS NUMBOLETOS,
--    SALA.NOMBRE AS SALA,
--	VERSION.IDIOMA AS IDIOMA,
--    STRING_AGG(ASIENTO.FILA + ASIENTO.NUMERO, ', ') AS ASIENTOS,
--	10 * COUNT(BOLETO.BOLETO_ID) AS TOTAL
--FROM BOLETO
--INNER JOIN USUARIO ON BOLETO.USUARIO_ID = USUARIO.USUARIO_ID
--INNER JOIN HORARIO_PELICULA ON BOLETO.HORARIO_ID = HORARIO_PELICULA.HORARIO_ID
--INNER JOIN ASIENTO ON BOLETO.ASIENTO_ID = ASIENTO.ASIENTO_ID
--INNER JOIN SALA ON SALA.SALA_ID = HORARIO_PELICULA.SALA_ID
--INNER JOIN CINE ON CINE.CINE_ID = SALA.CINE_ID
--INNER JOIN PELICULA ON PELICULA.PELICULA_ID = HORARIO_PELICULA.PELICULA_ID
--INNER JOIN VERSION on HORARIO_PELICULA.VERSION_ID = VERSION.VERSION_ID
--GROUP BY 
--	USUARIO.USUARIO_ID,
--    PELICULA.TITULO,
--    CINE.NOMBRE,
--    HORARIO_PELICULA.HORA_FUNCION,
--    SALA.NOMBRE,
--	VERSION.IDIOMA;
--GO

GO
INSERT [dbo].[ASIENTO] ([ASIENTO_ID], [SALA_ID], [HORARIO_ID], [NUMERO], [FILA], [DISPONIBLE]) VALUES (1, 2, 1, N'5', N'A', 0)
INSERT [dbo].[ASIENTO] ([ASIENTO_ID], [SALA_ID], [HORARIO_ID], [NUMERO], [FILA], [DISPONIBLE]) VALUES (2, 2, 1, N'6', N'A', 0)
INSERT [dbo].[ASIENTO] ([ASIENTO_ID], [SALA_ID], [HORARIO_ID], [NUMERO], [FILA], [DISPONIBLE]) VALUES (3, 2, 1, N'6', N'E', 0)
INSERT [dbo].[ASIENTO] ([ASIENTO_ID], [SALA_ID], [HORARIO_ID], [NUMERO], [FILA], [DISPONIBLE]) VALUES (4, 2, 1, N'7', N'E', 0)
GO
INSERT [dbo].[BOLETO] ([BOLETO_ID], [USUARIO_ID], [HORARIO_ID], [ASIENTO_ID], [FECHA_COMPRA], [ESTADO]) VALUES (1, 2, 1, 1, CAST(N'2025-04-23T10:25:04.833' AS DateTime), N'Confirmado')
INSERT [dbo].[BOLETO] ([BOLETO_ID], [USUARIO_ID], [HORARIO_ID], [ASIENTO_ID], [FECHA_COMPRA], [ESTADO]) VALUES (2, 2, 1, 2, CAST(N'2025-04-23T10:25:04.990' AS DateTime), N'Confirmado')
INSERT [dbo].[BOLETO] ([BOLETO_ID], [USUARIO_ID], [HORARIO_ID], [ASIENTO_ID], [FECHA_COMPRA], [ESTADO]) VALUES (3, 1, 1, 3, CAST(N'2025-04-27T19:00:13.573' AS DateTime), N'Confirmado')
INSERT [dbo].[BOLETO] ([BOLETO_ID], [USUARIO_ID], [HORARIO_ID], [ASIENTO_ID], [FECHA_COMPRA], [ESTADO]) VALUES (4, 1, 1, 4, CAST(N'2025-04-27T19:00:13.657' AS DateTime), N'Confirmado')
GO
INSERT [dbo].[HORARIO_PELICULA] ([HORARIO_ID], [PELICULA_ID], [SALA_ID], [VERSION_ID], [HORA_FUNCION], [ASIENTOS_DISPONIBLES], [ESTADO]) VALUES (1, 939243, 2, 1, CAST(N'2025-04-16T08:00:00.000' AS DateTime), 120, N'A')
GO
INSERT [dbo].[USUARIO] ([USUARIO_ID], [NOMBRE], [EMAIL], [IMAGEN], [SALT], [PASS], [FECHA_CREACION]) VALUES (1, N'doryan', N'doryan@gmail.com', N'https://static.vecteezy.com/system/resources/previews/002/275/847/original/male-avatar-profile-icon-of-smiling-caucasian-man-vector.jpg', N'@?Tp:¥?&»?Ît?Ív?Ìòì=öUÂ&¡?_?Bß?"0g©¾56ø?¤9?$', 0xD4CFA2FD5BAA8620D5307BCEC8FE2A80E32B12460959552F22EB9C3B2C7AD4F63789BE761D0D4ABC6366C81FC5487B3D4628712AAED8E5B2AD7721462702C867, CAST(N'2025-03-14T13:03:34.887' AS DateTime))
INSERT [dbo].[USUARIO] ([USUARIO_ID], [NOMBRE], [EMAIL], [IMAGEN], [SALT], [PASS], [FECHA_CREACION]) VALUES (2, N'doryan', N'd1@gmail.com', N'https://www.pngmart.com/files/23/Profile-PNG-Photo.png', N'5?Xï¦@l¡Ù£?MsíÍ#AêXÆkÚ?ÞòìVa1çrîÃ?é8M]V¦u', 0x54380CF81BA68F741DE87A6E39533FA2EC4B74EAC73C0B5FCB636E52CDA98302FFD83B9AEEC8E63BE326730E39157AC886BDD53407D17DBFD03A0A9BE739B434, CAST(N'2025-04-23T09:56:31.640' AS DateTime))
INSERT [dbo].[USUARIO] ([USUARIO_ID], [NOMBRE], [EMAIL], [IMAGEN], [SALT], [PASS], [FECHA_CREACION]) VALUES (3, N'doryan', N'd2@gmail.com', N'https://www.pngmart.com/files/23/Profile-PNG-Photo.png', N'¹øzÆ	OÞj%xi?5{¶JY`/
à&òR?i0²Wo"±?-mâ´Ö¬t[d', 0xF510B135DF55B2EBDAD21951A29FD4B9EFC62B90E08463457C718C6D4723711BFB7220065516C4BF28ADE5A424EF08E90DE9ABCCA480AF9FDCD23415CB73E76B, CAST(N'2025-04-23T11:39:14.190' AS DateTime))
INSERT [dbo].[USUARIO] ([USUARIO_ID], [NOMBRE], [EMAIL], [IMAGEN], [SALT], [PASS], [FECHA_CREACION]) VALUES (4, N'prueba', N'p@gmail.com', N'https://cdn-icons-png.flaticon.com/512/3135/3135768.png', N'$iUÚGªSÛÊâÏ_g?ñ¼Á; ?DS¹´ö«¤sÙ÷ôI??^.ðZ''ë?¥A?R', 0x813157B0A2A434D2AC154F9F16FBB6563907C811F4E4D997650BB84156C59ECBA01E8277C6617A22F20A233965BEA1F1A43C09B94802D586E31D6A7826BCCADD, CAST(N'2025-04-23T12:46:22.200' AS DateTime))
INSERT [dbo].[USUARIO] ([USUARIO_ID], [NOMBRE], [EMAIL], [IMAGEN], [SALT], [PASS], [FECHA_CREACION]) VALUES (5, N'dapi', N'dapi@gmail.com', N'123.jpg', N'=æ?Ä[¤Ó?"±¡væE?ãBÆ
ý???möÃW¡û?Y¾1?ÐÁxðò64±¢?Æ', 0x5C078E6C002AD61A9B66067A204B1D4129728556A7921A976A94ACDA16698CC7DF9105E286ABE569E223874CB6246D6BD944349F0971A8B5298976F0D8AD926F, CAST(N'2025-04-26T21:22:19.333' AS DateTime))
