import React from 'react';
import { vi, describe, expect, test, beforeEach } from "vitest";
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PersonalReviewsTab from '../../../../src/Pages/PersonalAccount/PersonalReviewsTab/PersonalReviewsTab';
import { userService } from '../../../../src/services/user.service.js';
import { userEvent } from '@testing-library/user-event';

vi.mock('../../../../src/services/user.service.js');

describe('PersonalReviewsTab', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    userService.getReviewsPagesCount.mockResolvedValue(1);
    userService.getReviews.mockResolvedValue([{ id: 1, contentName: 'Hime-sama Love Life 2', text: ")))" }]);
  });

  test('renders the search input and sort select', async () => {
    render(<PersonalReviewsTab />);
    
    const searchInput = await screen.findByPlaceholderText('Поиск по слову');
    const sortSelect = await screen.findByText('Сортировать по:');
    expect(searchInput).toBeInTheDocument();
    expect(sortSelect).toBeInTheDocument();
  });

  test('fetches and displays reviews on search submit', async () => {
    const user = userEvent.setup();

    render(<PersonalReviewsTab />);

    await user.type(screen.getByPlaceholderText('Поиск по слову'), 'test');
    await user.click(screen.getByAltText("Submit"));

    await waitFor(() => {
      expect(userService.getReviewsPagesCount).toHaveBeenLastCalledWith('test');
      expect(userService.getReviews).toHaveBeenCalledWith('rating', 'test', 1);
      expect(screen.getByText('Hime-sama Love Life 2')).toBeInTheDocument();
    });
  });

  test('handles pagination correctly', async () => {
    const user = userEvent.setup();
    userService.getReviewsPagesCount.mockResolvedValue(3);
    userService.getReviews.mockResolvedValue([{ id: 1, contentName: 'Hime-sama Love Life 2', text: ")))" },
      { id: 2, contentName: 'Hime-sama Love Life 1', text: ")))" }
    ]);

    render(<PersonalReviewsTab />);

    await user.type(screen.getByPlaceholderText('Поиск по слову'), 'test');
    await user.click(screen.getByAltText("Submit"));

    await waitFor(() => {
      expect(screen.getByText('Hime-sama Love Life 2')).toBeInTheDocument();
    });

    await user.click(screen.getByText('2'));

    await waitFor(() => {
      expect(userService.getReviews).toHaveBeenCalledWith('rating', 'test', 2);
    });
  });
});
