CREATE TRIGGER trgInsertInfoUser
ON tUser
AFTER INSERT
AS
BEGIN
    INSERT INTO tInfoUser (user_id, fullname, sex, date_of_birth, nickname)
    SELECT
        inserted.user_id,
        '',
        0,
        GETDATE(),
        'default'
    FROM
        inserted;
END