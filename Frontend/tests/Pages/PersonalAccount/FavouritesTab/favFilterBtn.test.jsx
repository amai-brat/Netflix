import { render, screen } from '@testing-library/react';
import FavouritesFilterButton from "../../../../src/Pages/PersonalAccount/FavouritesTab/FavouritesFilterButton.jsx";


describe('FavouritesFilterButton', () => {
    test('RendersCorrect', () => {
        render(<FavouritesFilterButton />);

        const labelElement = screen.getByText(/фильтр/i);
        expect(labelElement).toBeInTheDocument();
        expect(labelElement).toHaveAttribute('id', 'favourites-filter-button');
    });
});