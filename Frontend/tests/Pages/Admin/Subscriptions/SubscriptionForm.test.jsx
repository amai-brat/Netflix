import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, test, vi } from "vitest";
import { SubscriptionForm } from "../../../../src/Pages/Admin/Subscriptions/components/SubscriptionForm";
import { SubscriptionsContext } from "../../../../src/Pages/Admin/Subscriptions/components/SubscriptionsContext";

vi.mock('../../../../src/services/admin.subscription.service.js');

describe('SubscriptionForm Component', () => {
  const setSubscriptions = vi.fn();

  test('renders form fields', () => {
    render(
      <SubscriptionsContext.Provider value={{setSubscriptions}}>
        <SubscriptionForm />
      </SubscriptionsContext.Provider>
    );

    expect(screen.getByLabelText('Название')).toBeInTheDocument();
    expect(screen.getByLabelText('Описание')).toBeInTheDocument();
    expect(screen.getByLabelText('Максимальное разрешение')).toBeInTheDocument();
    expect(screen.getByLabelText('Цена')).toBeInTheDocument();
  });

  test('shows validation errors for empty fields', async () => {
    render(
      <SubscriptionsContext.Provider value={{setSubscriptions}}>
        <SubscriptionForm />
      </SubscriptionsContext.Provider>
    );
    
    await userEvent.click(screen.getByText('Отправить'));
    
    const errors = await screen.findAllByText('Обязательное поле');
    expect(errors.length).toBeGreaterThan(0);
  });
});