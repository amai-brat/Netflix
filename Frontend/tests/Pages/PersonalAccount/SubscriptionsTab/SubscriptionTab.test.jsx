// Entry.test.jsx
import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import Entry from '../../../../src/Pages/PersonalAccount/SubscriptionsTab/Entry.jsx';
import { subscriptionService } from '../../../../src/services/subscription.service.js';
import { authenticationService } from '../../../../src/services/authentication.service.js';
import userEvent from '@testing-library/user-event';

vi.mock('../../../../src/services/subscription.service.js', () => ({
    subscriptionService: {
        getSubscriptionById: vi.fn(),
        unsubscribe: vi.fn(),
    },
}));

vi.mock('../../../../src/services/authentication.service.js', () => ({
    authenticationService: {
        refreshToken: vi.fn(),
    },
}));

vi.mock('../../../../src/Pages/PersonalAccount/SubscriptionsTab/ConfirmationModal.jsx', () => ({
    default: ({ isOpen, onRequestClose, onConfirm, isDataFetching, response }) => {
        if (!isOpen) return null;
        return (
            <div data-testid="confirmation-modal">
                <button onClick={onRequestClose}>Close</button>
                <button onClick={onConfirm}>Confirm</button>
                {isDataFetching && <span>Loading...</span>}
                {response && <span>{response}</span>}
            </div>
        );
    },
}));

describe('Entry Component', () => {
    const mockSetSubscriptions = vi.fn();

    const mockData = {
        subscriptionId: 'sub123',
        boughtAt: new Date('2023-01-01'),
        expiresAt: new Date('2024-01-01'),
    };

    const mockSubscriptionData = {
        name: 'Premium',
        max_resolution: '4K',
        description: 'Premium subscription with 4K support.',
    };

    beforeEach(() => {
        vi.resetAllMocks();

        subscriptionService.getSubscriptionById.mockResolvedValue({
            response: { ok: true },
            data: mockSubscriptionData,
        });

        subscriptionService.unsubscribe.mockResolvedValue({
            response: { ok: true },
            data: {},
        });

        authenticationService.refreshToken.mockResolvedValue();
    });

    const renderComponent = () => {
        return render(<Entry data={mockData} setSubscriptions={mockSetSubscriptions} />);
    };

    it('должен корректно рендерить данные подписки', async () => {
        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalledWith(mockData.subscriptionId);
        });

        expect(screen.getByText(mockSubscriptionData.name)).toBeInTheDocument();

        expect(screen.getByText(`Куплено: ${mockData.boughtAt.toLocaleString('en-US').slice(0, 10)}`)).toBeInTheDocument();
    });

    it('должен открывать модальное окно при клике на кнопку "Отказаться"', async () => {
        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalled();
        });

        const checkbox = screen.getByRole('checkbox');
        await userEvent.click(checkbox); // Раскрываем подробности

        const denyButton = screen.getByText('Отказаться');
        await userEvent.click(denyButton);

        expect(screen.getByTestId('confirmation-modal')).toBeInTheDocument();
    });

    it('должен закрывать модальное окно при клике на кнопку "Close"', async () => {
        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalled();
        });

        const checkbox = screen.getByRole('checkbox');
        await userEvent.click(checkbox);

        const denyButton = screen.getByText('Отказаться');
        await userEvent.click(denyButton);

        expect(screen.getByTestId('confirmation-modal')).toBeInTheDocument();

        const closeButton = screen.getByText('Close');
        await userEvent.click(closeButton);

        expect(screen.queryByTestId('confirmation-modal')).not.toBeInTheDocument();
    });

    it('должен успешно отписываться при подтверждении в модальном окне', async () => {
        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalled();
        });

        const checkbox = screen.getByRole('checkbox');
        await userEvent.click(checkbox);

        const denyButton = screen.getByText('Отказаться');
        await userEvent.click(denyButton);

        expect(screen.getByTestId('confirmation-modal')).toBeInTheDocument();

        const confirmButton = screen.getByText('Confirm');
        await userEvent.click(confirmButton);

        await waitFor(() => {
            expect(subscriptionService.unsubscribe).toHaveBeenCalledWith(mockData.subscriptionId);
        });

        expect(authenticationService.refreshToken).toHaveBeenCalled();

        expect(mockSetSubscriptions).toHaveBeenCalledWith(expect.any(Function));

        await waitFor(() => {
            expect(screen.getByText('Успех')).toBeInTheDocument();
        });

        const closeButton = screen.getByText('Close');
        await userEvent.click(closeButton);
        expect(screen.queryByTestId('confirmation-modal')).not.toBeInTheDocument();
    });

    it('должен обрабатывать ошибку при отписке', async () => {
        subscriptionService.unsubscribe.mockResolvedValue({
            response: { ok: false, status: 500 },
            data: {},
        });

        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalled();
        });

        const checkbox = screen.getByRole('checkbox');
        await userEvent.click(checkbox);

        const denyButton = screen.getByText('Отказаться');
        await userEvent.click(denyButton);

        expect(screen.getByTestId('confirmation-modal')).toBeInTheDocument();

        const confirmButton = screen.getByText('Confirm');
        await userEvent.click(confirmButton);

        await waitFor(() => {
            expect(subscriptionService.unsubscribe).toHaveBeenCalledWith(mockData.subscriptionId);
        });

        expect(authenticationService.refreshToken).not.toHaveBeenCalled();

        await waitFor(() => {
            expect(screen.getByText('Ошибка: 500')).toBeInTheDocument();
        });

        const closeButton = screen.getByText('Close');
        await userEvent.click(closeButton);
        expect(screen.queryByTestId('confirmation-modal')).not.toBeInTheDocument();
    });

    it('должен обрабатывать исключение при отписке', async () => {
        subscriptionService.unsubscribe.mockRejectedValue(new Error('Network Error'));

        renderComponent();

        await waitFor(() => {
            expect(subscriptionService.getSubscriptionById).toHaveBeenCalled();
        });

        const checkbox = screen.getByRole('checkbox');
        await userEvent.click(checkbox);

        const denyButton = screen.getByText('Отказаться');
        await userEvent.click(denyButton);

        expect(screen.getByTestId('confirmation-modal')).toBeInTheDocument();

        const confirmButton = screen.getByText('Confirm');
        await userEvent.click(confirmButton);

        await waitFor(() => {
            expect(subscriptionService.unsubscribe).toHaveBeenCalledWith(mockData.subscriptionId);
        });

        expect(authenticationService.refreshToken).not.toHaveBeenCalled();

        await waitFor(() => {
            expect(screen.getByText('Network Error')).toBeInTheDocument();
        });

        const closeButton = screen.getByText('Close');
        await userEvent.click(closeButton);
        expect(screen.queryByTestId('confirmation-modal')).not.toBeInTheDocument();
    });
});
