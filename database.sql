CREATE TABLE Resources (
    Id int IDENTITY(1, 1) PRIMARY KEY,
    Status int,
    LockExpirationTimeUtc datetime NULL,
    LockedById int NULL
);

CREATE TABLE Users (
    Id int IDENTITY(1, 1) PRIMARY KEY,
    Name nvarchar(255) NOT NULL UNIQUE,
    Password nvarchar(2500) NOT NULL,
    IsAdmin bit
)

//TODO: add foreign key constraint on LockedById, don't keep passwords as plaint text, save it as hash(password + salt) instead

INSERT Resources (Status)
VALUES (0)

INSERT Resources (Status)
VALUES (1)

INSERT INTO Users (Name, Password, IsAdmin)
VALUES ('John', 'secretpassword', 0)

INSERT INTO Users (Name, Password, IsAdmin)
VALUES ('Rebeca', 'hardtoguesspassword', 1)