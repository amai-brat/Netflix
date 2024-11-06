import { render, screen, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import SignUpSignIn from '../../../src/Pages/SignUp&SignIn/SignUpSignIn';
import { userService } from "../../../src/services/user.service.js";
import { vi, test, describe, expect, beforeEach } from 'vitest';
import {DataStoreProvider} from "../../../src/store/dataStoreProvider.jsx";

const mockNavigate = vi.fn()

vi.mock('../../../src/services/user.service.js', () => ({
    userService: {
        getPersonalInfo: vi.fn().mockResolvedValue({ response: { ok: false } })
    }
}));

vi.mock('react-router-dom', async () => {
    const actual = await vi.importActual('react-router-dom');

    return {
        ...actual, 
        useNavigate: () => mockNavigate
    };
});

describe('SignUpSignIn logic', () => {
    beforeEach(()=>{
        mockNavigate.mockClear();
    })

    test('Должен вызвать userService.getPersonalInfo при рендеринге', async () => {
        render(
            <DataStoreProvider>
              <MemoryRouter>
                <SignUpSignIn formType="signup" />
              </MemoryRouter>
            </DataStoreProvider>
        );
    
        expect(userService.getPersonalInfo).toHaveBeenCalled(); 
    });

    test('Должен отображать заголовок и кнопку "Зарегистрироваться" при formType = signup', () => {
        render(
            <DataStoreProvider>
                <MemoryRouter>
                    <SignUpSignIn formType="signup"/>
                </MemoryRouter>
            </DataStoreProvider>
        );

        expect(screen.getByText(/Зарегистрироваться/i, { selector: 'div' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /Зарегистрироваться/i })).toBeInTheDocument();
    });

    test('Должен отображать заголовок и кнопку "Войти" при formType = signin', () => {
        render(
            <DataStoreProvider>
                <MemoryRouter>
                    <SignUpSignIn formType="signin" />
                </MemoryRouter>
            </DataStoreProvider>
        );

        expect(screen.getByText(/Войти/i, { selector: 'div' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /Войти/i })).toBeInTheDocument();
    });

    test('Должен вызвать navigate на MainContent при успешном получении данных пользователя', async () => {
        userService.getPersonalInfo.mockResolvedValueOnce({ response: { ok: true } });

        render(
            <DataStoreProvider>
                <MemoryRouter>
                    <SignUpSignIn formType="signup" />
                </MemoryRouter>
            </DataStoreProvider>
        );

        await waitFor(() => {
            expect(mockNavigate).toHaveBeenCalledWith('/MainContent');
        })
    });

    test('Не должен вызывать navigate на MainContent при неуспешном получении данных пользователя', async () => {
        render(
            <DataStoreProvider>
                <MemoryRouter>
                    <SignUpSignIn formType="signup"/>
                </MemoryRouter>
            </DataStoreProvider>
        );

        await waitFor(() => {
            expect(mockNavigate).not.toHaveBeenCalled();
        })
    });
});
