-- Добавление нового значения 'reset_password' в enum email_template
ALTER TYPE notification_service.email_template ADD VALUE 'reset_password';
