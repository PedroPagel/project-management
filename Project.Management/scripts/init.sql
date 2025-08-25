CREATE TABLE TL_ROLE (
    id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_name VARCHAR(100) NOT NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE TL_USER (
    id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_full_name VARCHAR(150) NOT NULL,
    ds_email VARCHAR(150) NOT NULL,
    ds_password_hash NVARCHAR(MAX) NOT NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE TL_PROJECT (
    id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_name VARCHAR(100) NOT NULL,
    ds_description VARCHAR(500),
    ds_status VARCHAR(50) NOT NULL,
    dt_start DATETIME NOT NULL,
    dt_end DATETIME NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL
);

CREATE TABLE TL_TASK (
    id UNIQUEIDENTIFIER PRIMARY KEY,
    ds_title VARCHAR(150) NOT NULL,
    ds_description NVARCHAR(MAX),
    ds_status VARCHAR(50),
    dt_due DATETIME,
    ProjectId UNIQUEIDENTIFIER NOT NULL,
    AssignedUserId UNIQUEIDENTIFIER NULL,
    dh_created DATETIME NOT NULL,
    dh_updated DATETIME NULL,
    ds_id_user_updated VARCHAR(256) NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES TL_PROJECT(id),
    FOREIGN KEY (AssignedUserId) REFERENCES TL_USER(id)
);

CREATE TABLE TL_PROJECT_MEMBER (
    user_id UNIQUEIDENTIFIER NOT NULL,
    project_id UNIQUEIDENTIFIER NOT NULL,
    role_id UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (user_id, project_id),
    FOREIGN KEY (UserId) REFERENCES TL_USER(id),
    FOREIGN KEY (project_id) REFERENCES TL_PROJECT(id),
    FOREIGN KEY (role_id) REFERENCES TL_ROLE(id)
);
