import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { PasswordField } from '../../../../src/Pages/PersonalAccount/PersonalInfoTab/components/PasswordField.jsx';
import { userService } from '../../../../src/services/user.service.js';
import { beforeEach, describe, expect, test, vi } from 'vitest';

vi.mock('../../../../src/services/user.service.js', () => ({
    userService: {
        changePassword: vi.fn(),
    },
}));

describe('PasswordField Component', () => {
    beforeEach(() => {
        vi.clearAllMocks();

        userService.changePassword.mockResolvedValue({ response: { ok: true }, data: {} });
    });

    test('Открывает модальное окно при клике на кнопку "Изменить"', () => {
        render(<PasswordField />);

        // Находим кнопку "Изменить"
        const changeButton = screen.getByText('Изменить');
        fireEvent.click(changeButton);

        // Проверяем, что модальное окно открылось (текст заголовка модального окна отображается)
        expect(screen.getByText('Смена пароля')).toBeInTheDocument();
    });

    test('Успешная смена пароля при правильном вводе', async () => {
        render(<PasswordField />);

        // Открываем модальное окно
        fireEvent.click(screen.getByText('Изменить'));

        // Заполняем поля ввода
        fireEvent.change(screen.getByLabelText('Пароль'), { target: { value: 'OldPassword1!' } });
        fireEvent.change(screen.getByLabelText('Новый пароль'), { target: { value: 'NewPassword1!' } });
        fireEvent.change(screen.getByLabelText('Повторите новый пароль'), { target: { value: 'NewPassword1!' } });

        // Кликаем на "Сохранить"
        fireEvent.click(screen.getByText('Сохранить'));

        // Ожидаем вызов сервиса и успешное сообщение
        await waitFor(() => {
            expect(userService.changePassword).toHaveBeenCalledWith('OldPassword1!', 'NewPassword1!');
            expect(screen.getByText('Пароль поменялся')).toBeInTheDocument();
        });
    });

    test('Неудачная смена пароля при несовпадении паролей', async () => {
        render(<PasswordField />);

        // Открываем модальное окно
        fireEvent.click(screen.getByText('Изменить'));

        // Заполняем поля, где новый пароль и повтор нового пароля не совпадают
        fireEvent.change(screen.getByLabelText('Пароль'), { target: { value: 'OldPassword1!' } });
        fireEvent.change(screen.getByLabelText('Новый пароль'), { target: { value: 'NewPassword1!' } });
        fireEvent.change(screen.getByLabelText('Повторите новый пароль'), { target: { value: 'DifferentPassword1!' } });

        // Кликаем на "Сохранить"
        fireEvent.click(screen.getByText('Сохранить'));

        // Ожидаем появление ошибки о несовпадении паролей
        await waitFor(() => {
            expect(screen.getByText('Пароли не совпадают')).toBeInTheDocument();
            expect(userService.changePassword).not.toHaveBeenCalled();
        });
    });

    test('Ошибка при несоответствии пароля требованиям', async () => {
        render(<PasswordField />);

        // Открываем модальное окно
        fireEvent.click(screen.getByText('Изменить'));

        // Вводим новый пароль, который не соответствует требованиям (например, без заглавной буквы)
        fireEvent.change(screen.getByLabelText('Пароль'), { target: { value: 'OldPassword1!' } });
        fireEvent.change(screen.getByLabelText('Новый пароль'), { target: { value: 'weakpassword' } });
        fireEvent.change(screen.getByLabelText('Повторите новый пароль'), { target: { value: 'weakpassword' } });

        // Кликаем на "Сохранить"
        fireEvent.click(screen.getByText('Сохранить'));

        // Ожидаем появление ошибки о несоответствии пароля требованиям
        await waitFor(() => {
            expect(screen.getByText('Минимум 8 символов, хотя бы одна заглавная латинская буква, одна строчная латинская буква, одна цифра и спец. символ')).toBeInTheDocument();
            expect(userService.changePassword).not.toHaveBeenCalled();
        });
    });
});
