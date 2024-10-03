import { render, screen, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import SignUpSignIn from '../../../src/Pages/SignUp&SignIn/SignUpSignIn';
import { userService } from "../../../src/services/user.service.js";
import { vi, test, describe, expect, beforeEach } from 'vitest';

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

describe('SignUpSignIn correct logic', () => {
    beforeEach(()=>{
        mockNavigate.mockClear();
    })

    test('Должен вызвать userService.getPersonalInfo при рендеринге', async () => {
        render(
          <MemoryRouter>
            <SignUpSignIn formType="signup" />
          </MemoryRouter>
        );
    
        expect(userService.getPersonalInfo).toHaveBeenCalled(); 
    });

    test('Должен отображать заголовок и кнопку "Зарегистрироваться" при formType = signup', () => {
        render(
            <MemoryRouter>
                <SignUpSignIn formType="signup"/>
            </MemoryRouter>
        );

        expect(screen.getByText(/Зарегистрироваться/i, { selector: 'div' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /Зарегистрироваться/i })).toBeInTheDocument();
    });

    test('Должен отображать заголовок и кнопку "Войти" при formType = signin', () => {
        render(
            <MemoryRouter>
                <SignUpSignIn formType="signin" />
            </MemoryRouter>
        );

        expect(screen.getByText(/Войти/i, { selector: 'div' })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /Войти/i })).toBeInTheDocument();
    });

    test('Должен вызвать navigate на MainContent при успешном получении данных пользователя', async () => {
        userService.getPersonalInfo.mockResolvedValueOnce({ response: { ok: true } });

        render(
            <MemoryRouter>
                <SignUpSignIn formType="signup" />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(mockNavigate).toHaveBeenCalled();
        })
    });

    test('Не должен вызывать navigate на MainContent при неуспешном получении данных пользователя', async () => {
        render(
            <MemoryRouter>
                <SignUpSignIn formType="signup"/>
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(mockNavigate).not.toHaveBeenCalled();
        })
    });
});
