import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PersonalInfoTab from '../../../../src/Pages/PersonalAccount/PersonalInfoTab/PersonalInfoTab.jsx';
import { userService } from '../../../../src/services/user.service.js';
import { authenticationService } from '../../../../src/services/authentication.service.js';
import { beforeEach, describe, expect, test, vi } from 'vitest';

vi.mock('../../../../src/services/user.service.js');
vi.mock('../../../../src/services/authentication.service.js');

describe('PersonalInfoTab', () => {
    const mockUser = {
        nickname: 'TestUser',
        email: 'test@example.com',
        birthDay: '2000-01-01',
        profilePictureUrl: 'path/to/picture.jpg',
    };

    beforeEach(() => {
        userService.getPersonalInfo.mockResolvedValue({
            response: { ok: true },
            data: mockUser,
        });

        authenticationService.getWhetherTwoFactorEnabled.mockResolvedValue({
            response: { ok: true },
            data: false,
        });

        authenticationService.enableTwoFactorAuth.mockResolvedValue({
            response: { ok: true },
        });

        userService.changeProfilePicture.mockResolvedValue({
            response: { ok: true },
            data: { ...mockUser, profilePictureUrl: 'path/to/new-picture.jpg' },
        });
    });

    test('Рендерит информацию о пользователе', async () => {
        render(<PersonalInfoTab />);

        await waitFor(() => {
            expect(screen.getByText(mockUser.nickname)).toBeInTheDocument();
            expect(screen.getByText(mockUser.email)).toBeInTheDocument();
            expect(screen.getByText(mockUser.birthDay)).toBeInTheDocument();
        });
    });

    test('Изменяет аватар пользователя при вставке нового изображения', async () => {
        render(<PersonalInfoTab />);
        await waitFor(() => screen.getByText(mockUser.nickname));

        const inputFile = screen.getByTestId('file-input');
        const file = new File(['test'], 'test.png', { type: 'image/png' });

        fireEvent.change(inputFile, { target: { files: [file] } });

        await waitFor(() => {
            expect(userService.changeProfilePicture).toHaveBeenCalledWith(expect.any(FormData));
            expect(screen.getByRole('img')).toHaveAttribute('src', 'path/to/new-picture.jpg');
        });
    });

    test('Активирует двухфакторную аутентификацию', async () => {
        render(<PersonalInfoTab />);
        await waitFor(() => screen.getByText(/не активирована/i));

        const activateButton = screen.getByText(/активировать/i);
        fireEvent.click(activateButton);

        await waitFor(() => {
            expect(screen.getByText(/активирована/i)).toBeInTheDocument();
        });
    });
});
