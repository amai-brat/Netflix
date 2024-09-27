import { render, screen, fireEvent } from '@testing-library/react';
import { vi } from 'vitest';
import FavouriteTabSearchPanel from "../../../../src/Pages/PersonalAccount/FavouritesTab/FavouriteTabSearchPanel.jsx";

const mockSetFavourites = vi.fn();

const favourites = [
    { contentBase: { name: 'Item A' } },
    { contentBase: { name: 'Item a' } },
    { contentBase: { name: 'Item B' } },
    { contentBase: { name: 'Item C' } },
];

const setup = () => {
    render(
        <FavouriteTabSearchPanel favourites={favourites} setFavourites={mockSetFavourites} />
    );
};

describe('FavouriteTabSearchPanel', () => {
    beforeEach(() => {
        setup();
    });

    test('RendersSearchInputAndIconCorrect', () => {
        expect(screen.getByRole('textbox')).toBeInTheDocument();
        expect(screen.getByAltText(/search/i)).toBeInTheDocument();
    });

    test('FiltersFavouritesByNameOnSearch', () => {
        const searchInput = screen.getByRole('textbox');
        fireEvent.change(searchInput, { target: { value: 'Item a' } });
        
        const searchIcon = screen.getByAltText(/search/i);
        fireEvent.click(searchIcon);
        
        expect(mockSetFavourites).toHaveBeenCalledWith([
            favourites[0],
            favourites[1]
        ]);
    });

    test('DoesNotFilterFavouritesIfSearchInputIsEmpty', () => {
        const searchIcon = screen.getByAltText(/search/i);
        fireEvent.click(searchIcon);
        
        expect(mockSetFavourites).toHaveBeenCalledWith(favourites);
    });
});
