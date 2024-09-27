import { render, screen, fireEvent } from '@testing-library/react';
import { vi } from 'vitest';
import SelectionContentSearchPanel from "../../../src/Pages/SelectionContent/SelectionContentSearchPanel.jsx";

const mockSetFilter = vi.fn();
const mockOnFilterApply = vi.fn();

const setup = (filter) => {
    render(
        <SelectionContentSearchPanel filter={filter} setFilter={mockSetFilter} onFilterApply={mockOnFilterApply} />
    );
}

describe('SelectionContentSearchPanel', () => {

    test('RendersSearchBarAndIcon', () => {
        setup({ name: '' })

        expect(screen.getByPlaceholderText('Поиск по названию')).toBeInTheDocument();
        expect(screen.getByAltText('Search')).toBeInTheDocument();
    });

    test('UpdatesFilterOnSearchBarChange', () => {
        const filter = { name: '' }
        setup(filter)
        const searchBar = screen.getByPlaceholderText('Поиск по названию');

        fireEvent.change(searchBar, { target: { value: 'Тест' } });

        expect(filter.name).toBe('Тест');
    });

    test('CallsSetFilterAndOnFilterApplyOnSearchIconClick', () => {
        setup({ name: 'Тест' })
        const searchIcon = screen.getByAltText('Search');

        fireEvent.click(searchIcon);

        expect(mockSetFilter).toHaveBeenCalledWith({ name: 'Тест' });
        expect(mockOnFilterApply).toHaveBeenCalled();
    });
});
