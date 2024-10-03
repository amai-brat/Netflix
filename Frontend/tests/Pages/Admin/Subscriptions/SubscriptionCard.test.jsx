import { beforeEach, describe, expect, test, vi } from "vitest";
import { SubscriptionCard } from "../../../../src/Pages/Admin/Subscriptions/components/SubscriptionCard";
import { render } from "@testing-library/react";
import { userEvent } from '@testing-library/user-event';
import Modal from 'react-modal';

vi.mock('../../../../src/Pages/Admin/Subscriptions/components/DeletionConfirmation.jsx', () => {
  return {DeletionConfirmation: () => <div id="delete-dialog">удалить</div>}
});

describe('SubscriptionCard Component', () => {
  const mockSetSubscriptionId = vi.fn();
  const mockSetContentType = vi.fn();
  const subscription = {
      id: '1',
      name: 'Test Subscription',
      description: 'Test Description',
      maxResolution: '1080p',
      price: '10.00'
  };

  beforeEach(() => {
    Modal.setAppElement(document.createElement("div"));
  });

  test('renders subscription name', () => {
      const { getByText } = render(
          <SubscriptionCard 
              subscription={subscription} 
              setSubscriptionId={mockSetSubscriptionId} 
              setContentType={mockSetContentType} 
          />
      );
      expect(getByText('Test Subscription')).toBeInTheDocument();
  });

  test('opens and closes content on title click', async () => {
      const { getByText, queryByText } = render(
          <SubscriptionCard 
              subscription={subscription} 
              setSubscriptionId={mockSetSubscriptionId} 
              setContentType={mockSetContentType} 
          />
      );

      await userEvent.click(getByText('Test Subscription'));
      expect(getByText('Описание: Test Description')).toBeInTheDocument();

      await userEvent.click(getByText('Test Subscription'));
      expect(queryByText('Описание: Test Description')).not.toBeInTheDocument();
  });

  test('calls setSubscriptionId and setContentType on edit button click', async () => {
      const { getByAltText, getByText } = render(
          <SubscriptionCard 
              subscription={subscription} 
              setSubscriptionId={mockSetSubscriptionId} 
              setContentType={mockSetContentType} 
          />
      );

      await userEvent.click(getByText('Test Subscription'));

      await userEvent.click(getByAltText('Edit'));
      expect(mockSetSubscriptionId).toHaveBeenCalledWith(subscription.id);
      expect(mockSetContentType).toHaveBeenCalledWith('edit');
  });

  test('opens delete modal on delete button click', async () => {
      const { getByAltText, getByText } = render(
          <SubscriptionCard 
              subscription={subscription} 
              setSubscriptionId={mockSetSubscriptionId} 
              setContentType={mockSetContentType} 
          />
      );

      await userEvent.click(getByText('Test Subscription'));
      await userEvent.click(getByAltText('Del'));

      expect(getByText(/Удалить/i)).toBeInTheDocument();
  });
});

