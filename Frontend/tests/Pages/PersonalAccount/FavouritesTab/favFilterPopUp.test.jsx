import { render, screen, fireEvent } from '@testing-library/react';
import { describe, test, vi, expect, beforeEach } from "vitest";
import FavouritesFilterPopUp from "../../../../src/Pages/PersonalAccount/FavouritesTab/FavouritesFilterPopUp.jsx";

const mockSetFavourites = vi.fn(); // Создаем мок-функцию для setFavourites

const favourites = [
    { contentBase: { name: 'Item B' }, score: 1, addedAt: '2023-01-02' },
    { contentBase: { name: 'Item A' }, score: 2, addedAt: '2023-01-01' },
    { contentBase: { name: 'Item C' }, score: 3, addedAt: '2023-01-03' },
];

const setup = () => {
    render(
        <FavouritesFilterPopUp favourites={favourites} setFavourites={mockSetFavourites} />
    );
};

describe('FavouritesFilterPopUp', () => {
    beforeEach(() => {
        setup();
    });

    test('RendersAllFilterOptions', () => {
        expect(screen.getByText(/по названию/i)).toBeInTheDocument();
        expect(screen.getByText(/по оценке/i)).toBeInTheDocument();
        expect(screen.getByText(/по дате/i)).toBeInTheDocument();
    });
    
    test('FiltersFavouritesByName', () => {
        const nameFilter = screen.getByText(/по названию/i);
        fireEvent.click(nameFilter);

        expect(mockSetFavourites).toHaveBeenCalledWith([
            favourites[1],
            favourites[0],
            favourites[2]
        ]);
    });

    test('FiltersFavouritesByScore', () => {
        const scoreFilter = screen.getByText(/по оценке/i);
        fireEvent.click(scoreFilter);

        expect(mockSetFavourites).toHaveBeenCalledWith([
            favourites[2],
            favourites[1],
            favourites[0]
        ]);
    });

    test('FiltersFavouritesByDate', () => {
        const dateFilter = screen.getByText(/по дате/i);
        fireEvent.click(dateFilter);

        expect(mockSetFavourites).toHaveBeenCalledWith([
            favourites[2],
            favourites[0],
            favourites[1]
        ]);
    });
});
