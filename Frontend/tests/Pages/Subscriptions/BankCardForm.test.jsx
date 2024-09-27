import React from 'react';
import { render, fireEvent, waitFor, screen, act } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import { subscriptionService } from '../../../src/services/subscription.service';
import { expect, test, vi } from 'vitest';
import userEvent from '@testing-library/user-event';
import { BankCardForm } from '../../../src/Pages/Subscriptions/components/BankCardForm';
import { authenticationService } from '../../../src/services/authentication.service';

vi.mock('../../../src/services/subscription.service');
vi.mock('../../../src/services/authentication.service');

describe('BankCardForm', () => {
    const setup = () => {
        return render(
            <Router>
                <BankCardForm subscriptionId="123" />
            </Router>
        );
    };

    test('renders form fields', () => {
        const { getByLabelText } = setup();
        expect(getByLabelText(/номер карты/i)).toBeInTheDocument();
        expect(getByLabelText(/фио обладателя карты/i)).toBeInTheDocument();
        expect(getByLabelText(/срок/i)).toBeInTheDocument();
        expect(getByLabelText(/cvc\/cvv/i)).toBeInTheDocument();
    });

    test('shows validation errors on submit with empty fields', async () => {
        const { getByRole } = setup();
        fireEvent.click(getByRole('button', { name: /отправить/i }));

        const errors = await screen.findAllByText(/обязательное поле/i);
        expect(errors.length).toBeGreaterThan(0);
    });

    test('shows specific validation error for card number', async () => {
        const user = userEvent.setup();
        const { getByLabelText, getByRole } = setup();

        await user.type(getByLabelText(/номер карты/i), "1213123");
        await user.click(getByRole('button', { name: /отправить/i }));
        
        const error = await screen.findByText(/номер карты состоит из 16 цифр/i);
        expect(error).toBeInTheDocument();
    });

    test('submits form successfully', async () => {
        const user = userEvent.setup();
        subscriptionService.buySubscription.mockResolvedValueOnce({ response: { ok: true }, data: {} });
        authenticationService.refreshToken.mockResolvedValueOnce();

        const { getByLabelText, getByRole } = setup();

        await user.type(getByLabelText(/номер карты/i), '1234567812345678');
        await user.type(getByLabelText(/фио обладателя карты/i), 'Иванов Иван');
        await user.type(getByLabelText(/срок/i),'12/12');
        await user.type(getByLabelText(/cvc\/cvv/i),'123');

        await user.click(getByRole('button', { name: /отправить/i }));
        
        const success = await screen.findByText(/успешная покупка/i);
        expect(success).toBeInTheDocument();
    });

    test('shows error message on failed submission', async () => {
        const user = userEvent.setup();
        subscriptionService.buySubscription.mockResolvedValueOnce({ response: { ok: false }, data: {} });

        const { getByLabelText, getByRole } = setup();

        await user.type(getByLabelText(/номер карты/i), '1234567812345678');
        await user.type(getByLabelText(/фио обладателя карты/i), 'Иванов Иван');
        await user.type(getByLabelText(/срок/i),'12/12');
        await user.type(getByLabelText(/cvc\/cvv/i),'123');

        await user.click(getByRole('button', { name: /отправить/i }));
        
        const error = await screen.findByText(/ошибка/i);
        expect(error).toBeInTheDocument();
    });
});