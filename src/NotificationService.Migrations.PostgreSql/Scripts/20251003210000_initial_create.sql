create schema if not exists notification_service;

comment on schema notification_service is 'Схема для логирования отправки email сообщений.';

CREATE TYPE notification_service.email_template AS ENUM ('default', 'with_button');
COMMENT ON TYPE notification_service.email_template IS 'Перечисление доступных шаблонов email сообщений.';

create table if not exists notification_service.email_messages
(
    id serial primary key,
    author varchar(64) not null,
    subject varchar(64) not null,
    recipients text[] NOT NULL,
    email_template notification_service.email_template not null,
    template_params jsonb not null,
    send_time timestamptz not null
    );
COMMENT ON TABLE notification_service.email_messages IS 'Таблица для логирования результатов отправки email сообщений.';

COMMENT ON COLUMN notification_service.email_messages.id IS 'Уникальный идентификатор записи в логе.';
COMMENT ON COLUMN notification_service.email_messages.author IS 'Автор письма (отправитель).';
COMMENT ON COLUMN notification_service.email_messages.subject IS 'Тема письма.';
COMMENT ON COLUMN notification_service.email_messages.recipients IS 'Список получателей письма (через запятую или в формате JSON).';
COMMENT ON COLUMN notification_service.email_messages.email_template IS 'Шаблон письма из перечня email_template.';
COMMENT ON COLUMN notification_service.email_messages.template_params IS 'Параметры шаблона письма в формате JSONB (словарь ключ-значение).';
COMMENT ON COLUMN notification_service.email_messages.send_time IS 'Время отправки письма.';
