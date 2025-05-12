CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_name VARCHAR(100) NOT NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_full_name VARCHAR(150) NOT NULL,
    ds_email VARCHAR(150) NOT NULL,
    ds_password_hash NVARCHAR(MAX) NOT NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE Projects (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_name VARCHAR(100) NOT NULL,
    ds_description VARCHAR(500),
    ds_status VARCHAR(50) NOT NULL,
    dt_start DATETIME NOT NULL,
    dt_end DATETIME NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_title VARCHAR(150) NOT NULL,
    ds_description NVARCHAR(MAX),
    ds_status VARCHAR(50),
    dt_due DATETIME,
    ProjectId UNIQUEIDENTIFIER NOT NULL,
    AssignedUserId UNIQUEIDENTIFIER NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (AssignedUserId) REFERENCES Users(Id)
);

CREATE TABLE ProjectMembers (
    UserId UNIQUEIDENTIFIER NOT NULL,
    ProjectId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (UserId, ProjectId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
