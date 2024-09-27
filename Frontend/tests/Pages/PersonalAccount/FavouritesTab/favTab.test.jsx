import {render, screen, fireEvent, waitFor} from '@testing-library/react';
import { vi } from 'vitest';
import FavouritesTab from "../../../../src/Pages/PersonalAccount/FavouritesTab/FavouritesTab.jsx";
import {userService} from "../../../../src/services/user.service.js";
import {BrowserRouter} from "react-router-dom";

vi.mock("../../../../src/services/user.service.js");

const mockFavourites = [
    { contentBase: {id: '1', name: 'Item 1', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-01', score: 5 },
    { contentBase: {id: '2', name: 'Item 2', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-02', score: 4 },
    { contentBase: {id: '3', name: 'Item 3', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-03', score: 3 },
];

const mockFavouritesExtra = [...mockFavourites,
    { contentBase: {id: '4', name: 'Item 4', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-04', score: 2 },
    { contentBase: {id: '5', name: 'Item 5', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-05', score: 1 },
    { contentBase: {id: '6', name: 'Item 6', posterUrl: 'https://example.com/poster.jpg' }, addedAt: '2023-01-06', score: 0 },
];

const setup = () => {
    render(
        <BrowserRouter>
            <FavouritesTab />
        </BrowserRouter>
    );
};

describe('FavouritesTab', () => {
    test('DisplaysLoadingMessageWhenFavouritesAreUndefined', () => {
        userService.getFavourites.mockResolvedValue({ response: { ok: true }, data: undefined });
        setup();
        
        expect(screen.getByText(/Ищем \. \. \./i)).toBeInTheDocument();
    });

    test('DisplaysErrorMessageWhenFavouritesAreNull', async () => {
        userService.getFavourites.mockResolvedValue({ response: { ok: false }, data: null });
        setup();

        await waitFor(() => {
            expect(screen.getByText(/Что-то не так, мы ничего не нашли/i)).toBeInTheDocument();
        });
    });

    test('DisplaysMessageWhenFavouritesAreEmpty', async () => {
        userService.getFavourites.mockResolvedValue({ response: { ok: true }, data: [] });
        setup();

        await waitFor(() => {
            expect(screen.getByText(/У вас нет избранного контента/i)).toBeInTheDocument();
        });
    });

    test('DisplaysFilteredFavourites', async () => {
        userService.getFavourites.mockResolvedValue({ response: { ok: true }, data: mockFavourites });
        setup();

        await waitFor(() => {
            expect(screen.getByText(/Item 1/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 2/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 3/i)).toBeInTheDocument();
        });
    });

    test('ResetsFiltersAndShowsAllFavourites', async () => {
        userService.getFavourites.mockResolvedValue({ response: { ok: true }, data: mockFavourites });
        setup();
        
        await screen.findByText(/Item 1/i); //Почему-то без этого не проходит
        await waitFor(() => {
            const searchInput = screen.getByRole('textbox');
            fireEvent.change(searchInput, { target: { value: 'Item 1' } });
    
            const searchIcon = screen.getByAltText(/search/i);
            fireEvent.click(searchIcon);
    
            expect(screen.getByText(/Item 1/i)).toBeInTheDocument();
            
            const resetButton = screen.getByAltText(/Reset/i);
            fireEvent.click(resetButton);
    
            expect(screen.getByText(/Item 1/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 2/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 3/i)).toBeInTheDocument();
        });
    });
    
    test('DisplaysPaginatedContentWhenThereAreMoreThan5Favourites', async () => {
        userService.getFavourites.mockResolvedValueOnce({response: { ok: true },data: mockFavouritesExtra });
        setup()

        await waitFor(() => {
            expect(screen.getByText(/Item 1/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 2/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 3/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 4/i)).toBeInTheDocument();
            expect(screen.getByText(/Item 5/i)).toBeInTheDocument();
        });

        // Пагинация
        const secondPageButton = screen.getByText('2');
        expect(secondPageButton).toBeInTheDocument();
        fireEvent.click(secondPageButton);

        await waitFor(() => {
            expect(screen.getByText(/Item 6/i)).toBeInTheDocument();
        });
    });
});
