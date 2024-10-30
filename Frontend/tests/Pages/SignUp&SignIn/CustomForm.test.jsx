import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { vi, test, describe, expect } from 'vitest';
import { MemoryRouter } from 'react-router-dom';
import { CustomForm } from '../../../src/Pages/SignUp&SignIn/CustomForm'
import { authenticationService } from '../../../src/services/authentication.service';
import {DataStoreProvider} from "../../../src/store/dataStoreProvider.jsx";

vi.mock('../../../src/services/authentication.service', () => ({
    authenticationService: {
        signup: vi.fn().mockResolvedValue({ ok: true, data: '' }),
        signin: vi.fn().mockResolvedValue({ ok: true, data: '' }),
    }
}));

describe('CustomForm Signup', () => {
    test('Показывает обязательные для заполнения поля', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signup" /></MemoryRouter></DataStoreProvider>);
    
        fireEvent.submit(container.querySelector('form'));

        await waitFor(() => {
            expect(screen.getAllByText(/обязательное поле/i)).toHaveLength(3);
        })
    });

    test('Выводит ошибку неправильного формата почты', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signup" /></MemoryRouter></DataStoreProvider>);

        fireEvent.input(screen.getByPlaceholderText(/почта/i), {
            target: { value: 'invalid-email' },
        });

        fireEvent.submit(container.querySelector('form'));

        await waitFor(() => {
            expect(screen.getByText(/неправильный адрес почты/i)).toBeInTheDocument();
        })
    });

    test('Выводит ошибку неправильного формата логина', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signup" /></MemoryRouter></DataStoreProvider>);

        const invalidLogins = ['abc', 'thisstringismorethan26symbols', 'кириллица;}{']
        const errors = ['Минимальная длина логина - 4 символов', 'Максимальная длина логина - 25 символов', 'Запрещенные символы']

        for (let i = 0; i < invalidLogins.length; i++){
            fireEvent.input(screen.getByPlaceholderText(/логин/i), {
                target: { value: invalidLogins[i] },
            });

            fireEvent.submit(container.querySelector('form'));

            await waitFor(() => {
                expect(screen.getByText(errors[i])).toBeInTheDocument();
            })
        }
    });

    test('Выводит ошибку неправильного формата пароля', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signup" /></MemoryRouter></DataStoreProvider>);

        const invalidPasswords = ['abcd', 'thisstringismorethan30symbols;!', 'pswNoSpecSymbol']
        const errors = ['Минимальная длина пароля - 8 символов', 'Максимальная длина пароля - 30 символов',
             'Пароль должен содержать хотя бы одну букву, цифру и спецсимвол']

        for (let i = 0; i < invalidPasswords.length; i++){
            fireEvent.input(screen.getByPlaceholderText(/пароль/i), {
                target: { value: invalidPasswords[i] },
            });

            fireEvent.submit(container.querySelector('form'));

            await waitFor(() => {
                expect(screen.getByText(errors[i])).toBeInTheDocument();
            })
        }
    });

    test('Успешное отправление формы при правильных данных', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signup" /></MemoryRouter></DataStoreProvider>);

        fireEvent.input(screen.getByPlaceholderText(/логин/i), {
            target: { value: 'CorrectLogin' },
        });

        fireEvent.input(screen.getByPlaceholderText(/почта/i), {
            target: { value: 'correctLogin@mail.ru' },
        });

        fireEvent.input(screen.getByPlaceholderText(/пароль/i), {
            target: { value: 'correctPassword228;' },
        });

        fireEvent.submit(container.querySelector('form'));

        await waitFor(() => {
            expect(authenticationService.signup).toHaveBeenCalledWith({
                login: 'CorrectLogin',
                email: 'correctLogin@mail.ru',
                password: 'correctPassword228;'
            })

            expect(screen.getByText(/успешная регистрация/i)).toBeInTheDocument();
        })
    })
});

describe('CustomForm SignIn', () => {
    test('Показывает обязательные для заполнения поля', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signin" /></MemoryRouter></DataStoreProvider>);
    
        fireEvent.submit(container.querySelector('form'));

        await waitFor(() => {
            expect(screen.getAllByText(/обязательное поле/i)).toHaveLength(2);
        })
    });

    test('Успешное отправление формы при правильных данных', async () => {
        const {container} = render(<DataStoreProvider><MemoryRouter><CustomForm formType="signin" /></MemoryRouter></DataStoreProvider>);

        fireEvent.input(screen.getByPlaceholderText(/почта/i), {
            target: { value: 'correctLogin@mail.ru' },
        });

        fireEvent.input(screen.getByPlaceholderText(/пароль/i), {
            target: { value: 'correctPassword228;' },
        });

        fireEvent.submit(container.querySelector('form'));

        await waitFor(() => {
            expect(authenticationService.signin).toHaveBeenCalledWith({
                email: 'correctLogin@mail.ru',
                password: 'correctPassword228;',
                rememberMe: false
            })

            expect(screen.getByText(/успешный вход/i)).toBeInTheDocument();
        })
    })
})