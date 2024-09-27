import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import SelectionContentCard from "../../../src/Pages/SelectionContent/SelectionContentCard.jsx";

const mockContent = {
    id: 1,
    name: 'Test Content',
    posterUrl: 'https://example.com/poster.jpg',
};

const setup = (content) => {
    render(
        <BrowserRouter>
            <SelectionContentCard content={content} />
        </BrowserRouter>
    );
};

describe('SelectionContentCard', () => {

    beforeEach(() => {
        setup(mockContent);
    });

    test('RendersSelectionContentCardWithPosterAndName', () => {
        expect(screen.getByText('Test Content')).toBeInTheDocument();
        const img = screen.getByAltText('Poster');
        expect(img).toHaveAttribute('src', mockContent.posterUrl);
    });

    test('SetsDefaultPosterOnImageError', () => {
        const img = screen.getByAltText('Poster');
        fireEvent.error(img);

        expect(img).toHaveAttribute('src', '/src/assets/NoImage.svg');
    });

    test('NavigatesToViewContentOnClick', () => {
        const card = screen.getByText('Test Content');
        fireEvent.click(card);

        expect(window.location.pathname).toBe(`/ViewContent/${mockContent.id}`);
    });
});
