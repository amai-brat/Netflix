import { render, fireEvent, screen } from '@testing-library/react';
import {BrowserRouter} from 'react-router-dom';
import FavouriteContentCard from '../../../../src/Pages/PersonalAccount/FavouritesTab/FavouriteContentCard.jsx';
import {contentService} from "../../../../src/services/content.service.js";
import { describe, test, vi, expect, beforeEach} from "vitest";

vi.mock('../../../../src/services/content.service.js');

const mockContent = {
    id: '1',
    name: 'Test Content',
    posterUrl: 'https://example.com/poster.jpg',
};

const setup = () => {
    render(
        <BrowserRouter>
            <FavouriteContentCard content={mockContent} score={8} addedAt={new Date().toISOString()} />
        </BrowserRouter>
    );
    contentService.removeFromFavourites.mockResolvedValue({ response: { ok: true } });
};

describe('FavouriteContentCard', () => {
    beforeEach(() => {
        setup();
    });

    test('RendersContentCardWithCorrectData', () => {
        expect(screen.getByAltText('Poster')).toHaveAttribute('src', mockContent.posterUrl);
        expect(screen.getByText(mockContent.name)).toBeInTheDocument();
        expect(screen.getByText(/Ваша оценка: 8 \/ 10/i)).toBeInTheDocument();
    });

    test('NavigatesToViewContentOnPosterClick', () => {
        const img = screen.getByAltText('Poster');
        fireEvent.click(img);
        expect(window.location.pathname).toBe(`/ViewContent/${mockContent.id}`);
    });

    test('NavigatesToViewContentOnLabelClick', () => {
        const nameLabel = screen.getByText(mockContent.name);
        fireEvent.click(nameLabel);
        expect(window.location.pathname).toBe(`/ViewContent/${mockContent.id}`);
    });

    test('RemovesContentFromFavourites', async () => {
        const removeButton = screen.getByAltText('Remove');
        fireEvent.click(removeButton);

        expect(contentService.removeFromFavourites).toHaveBeenCalledWith(mockContent.id);
        expect(await screen.findByText(/Test Content/i)).not.toBeVisible();
    });

    test('HandlesImageErrorAndSetsDefaultImage', () => {
        const img = screen.getByAltText('Poster');
        fireEvent.error(img);

        expect(img).toHaveAttribute('src', '/src/assets/NoImage.svg');
    });
    
    test('ShowsTheCardAgainIfRemoveFails', async () => {
        contentService.removeFromFavourites.mockResolvedValue({ response: { ok: false } });

        fireEvent.click(screen.getByAltText('Remove'));

        expect(contentService.removeFromFavourites).toHaveBeenCalledWith(mockContent.id);
        expect(await screen.findByText(/Test Content/i)).toBeVisible();
    });
});