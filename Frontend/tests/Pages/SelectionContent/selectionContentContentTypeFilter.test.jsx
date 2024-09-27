import { render, screen, fireEvent } from '@testing-library/react';
import SelectionContentContentTypeFilter from "../../../src/Pages/SelectionContent/SelectionContentContentTypeFilter.jsx";

const mockFilter = { types: [] };

const setup = (contentTypes) => {
    render(
        <SelectionContentContentTypeFilter filter={mockFilter} contentTypes={contentTypes} />
    );
};

const contentTypes = [
    { id: 1, contentType: 'Type 1' },
    { id: 2, contentType: 'Type 2' },
];

describe('SelectionContentContentTypeFilter', () => {

    test('RendersLoadingState', () => {
        setup(undefined);
        
        expect(screen.getByText('Загружаем')).toBeInTheDocument();
    });

    test('RendersErrorState', () => {
        setup(null);
        
        expect(screen.getByText('Что-то пошло не так')).toBeInTheDocument();
    });

    test('RendersContentTypes', () => {
        setup(contentTypes)
        
        expect(screen.getByText('Type 1')).toBeInTheDocument();
        expect(screen.getByText('Type 2')).toBeInTheDocument();
    });
});
