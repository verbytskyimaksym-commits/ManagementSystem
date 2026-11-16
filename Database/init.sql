PRAGMA foreign_keys = ON;

-- 1. Лікарі (відповідає Users)
CREATE TABLE IF NOT EXISTS Doctors (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL
);

-- 2. Графіки лікарів (відповідає Items)
CREATE TABLE IF NOT EXISTS Schedules (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    DoctorID INTEGER NOT NULL,
    Date TEXT NOT NULL,
    TimeSlot TEXT NOT NULL,
    Status TEXT NOT NULL DEFAULT 'Available',
    FOREIGN KEY (DoctorID) REFERENCES Doctors(ID) ON DELETE CASCADE
);

-- 3. Записи пацієнтів (відповідає Actions)
CREATE TABLE IF NOT EXISTS Appointments (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    DoctorID INTEGER NOT NULL,
    ScheduleID INTEGER,
    PatientName TEXT NOT NULL,
    AppointmentDate TEXT NOT NULL,
    Status TEXT NOT NULL DEFAULT 'Pending',
    FOREIGN KEY (DoctorID) REFERENCES Doctors(ID) ON DELETE CASCADE,
    FOREIGN KEY (ScheduleID) REFERENCES Schedules(ID) ON DELETE SET NULL
);

-- Тестові дані: Лікарі
INSERT INTO Doctors (Name, Email, PasswordHash) VALUES
('Др. Олена Коваленко', 'kovalenko@clinic.ua', 'hash_doctor_1'),
('Др. Андрій Мельник', 'melnyk@clinic.ua', 'hash_doctor_2'),
('Др. Ірина Бондар', 'bondar@clinic.ua', 'hash_doctor_3');

-- Тестові дані: Графіки лікарів
INSERT INTO Schedules (DoctorID, Date, TimeSlot, Status) VALUES
(1, '2026-11-20', '09:00 - 10:00', 'Available'),
(1, '2026-11-20', '10:00 - 11:00', 'Booked'),
(2, '2026-11-21', '12:00 - 13:00', 'Available');

-- Тестові дані: Записи на прийом
INSERT INTO Appointments (DoctorID, ScheduleID, PatientName, AppointmentDate, Status) VALUES
(1, 2, 'Іван Петренко', '2026-11-20 10:00', 'Confirmed'),
(1, 1, 'Марія Сидоренко', '2026-11-20 09:00', 'Pending'),
(2, 3, 'Олег Ткаченко', '2026-11-21 12:00', 'Pending');