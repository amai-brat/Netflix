import { render, screen } from '@testing-library/react';
import { describe, test, expect } from "vitest";
import SelectionContentGenresFilter from "../../../src/Pages/SelectionContent/SelectionContentGenresFilter.jsx";

const mockFilter = { genres: [] };

const setup = (genres) => {
    render(
        <SelectionContentGenresFilter filter={mockFilter} genres={genres} />
    );
};

const genres = [
    { id: 1, name: 'Genre 1' },
    { id: 2, name: 'Genre 2' },
];

describe('SelectionContentGenreFilter', () => {

    test('RendersLoadingState', () => {
        setup(undefined);

        expect(screen.getByText('Загружаем')).toBeInTheDocument();
    });

    test('RendersErrorState', () => {
        setup(null);

        expect(screen.getByText('Что-то пошло не так')).toBeInTheDocument();
    });

    test('RendersContentTypes', () => {
        setup(genres)

        expect(screen.getByText('Genre 1')).toBeInTheDocument();
        expect(screen.getByText('Genre 2')).toBeInTheDocument();
    });
});
