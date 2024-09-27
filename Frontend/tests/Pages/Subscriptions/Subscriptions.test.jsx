import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import Subscriptions from '../../../src/Pages/Subscriptions/Subscriptions';
import { subscriptionService } from '../../../src/services/subscription.service';
import Modal from 'react-modal';
import { MemoryRouter } from 'react-router-dom';
import { beforeEach, describe, expect, test, vi } from 'vitest';

vi.mock('../../../src/services/subscription.service');

describe('Subscriptions Page', () => {
    beforeEach(() => {
        vi.clearAllMocks();
        Modal.setAppElement(document.createElement("div"));
    });

    test('fetches and displays subscriptions', async () => {
        subscriptionService.getAllSubscriptions.mockResolvedValueOnce({
            response: { ok: true },
            data: [
                { id: 1, name: 'Subscription 1' },
                { id: 2, name: 'Subscription 2' },
            ],
        });

        subscriptionService.getCurrentSubscriptions.mockResolvedValueOnce({
            response: { ok: true },
            data: [
                { subscriptionId: 1 },
            ],
        });

        render(<Subscriptions />);

        await waitFor(() => {
            expect(screen.getByText('Subscription 1')).toBeInTheDocument();
            expect(screen.getByText('Subscription 2')).toBeInTheDocument();
        });
    });

    test('opens modal on subscription click', async () => {
        subscriptionService.getAllSubscriptions.mockResolvedValueOnce({
            response: { ok: true },
            data: [
                { id: 1, name: 'Subscription 1' },
            ],
        });

        subscriptionService.getCurrentSubscriptions.mockResolvedValueOnce({
            response: { ok: true },
            data: [],
        });

        render(<MemoryRouter><Subscriptions /></MemoryRouter>);

        await waitFor(() => {
            expect(screen.getByText('Subscription 1')).toBeInTheDocument();
        });

        fireEvent.click(screen.getByText('Купить'));

        expect(screen.getByText('Вы покупаете:')).toBeInTheDocument();
    });
});
