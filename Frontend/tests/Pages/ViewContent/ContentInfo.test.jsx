import { render, screen, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import ContentInfo from '../../../src/Pages/ViewContent/ContentInfo.jsx';
import { contentService } from '../../../src/services/content.service.js';
import { toast } from 'react-toastify';
import userEvent from '@testing-library/user-event';

vi.mock('react-toastify', () => ({
    toast: {
        success: vi.fn(),
        error: vi.fn(),
    },
}));

vi.mock('../../../src/services/content.service.js', () => ({
    contentService: {
        addToFavourites: vi.fn(),
        removeFromFavourites: vi.fn(),
    },
}));

describe('ContentInfo Component', () => {
    const mockContentData = {
        id: 1,
        name: 'Test Movie',
        description: 'A test movie description.',
        posterUrl: 'http://example.com/poster.jpg',
        trailerInfo: {
            name: 'Trailer 1',
            url: 'http://example.com/trailer',
        },
        country: 'USA',
        slogan: 'Test Slogan',
        genres: [{ name: 'Action' }, { name: 'Adventure' }],
        yearRange: { start: 2020, end: 2021 },
        seasonInfos: [{}, {}],
        contentType: { contentTypeName: 'Movie' },
        releaseDate: '2020-05-20',
        movieLength: 120,
        budget: { budgetValue: 1000000, budgetCurrencyName: 'USD' },
        ageRatings: { age: 16, ageMpaa: 'PG-13' },
        ratings: {
            imdbRating: 7.5,
            kinopoiskRating: 8.0,
            localRating: 7.8,
        },
        personsInContent: [
            {
                name: 'Actor One',
                profession: { professionName: 'Актер' },
            },
            {
                name: 'Actor Two',
                profession: { professionName: 'Актер' },
            },
            {
                name: 'Director One',
                profession: { professionName: 'Режиссёр' },
            },
        ],
    };

    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('renders all main elements correctly', () => {
        render(<ContentInfo contentData={mockContentData} />);

        expect(screen.getByText(mockContentData.name)).toBeInTheDocument();

        expect(screen.getByText(mockContentData.description)).toBeInTheDocument();

        const posterImg = screen.getByAltText('poster');
        expect(posterImg).toHaveAttribute('src', mockContentData.posterUrl);

        expect(screen.getByText(mockContentData.trailerInfo.name)).toBeInTheDocument();

        expect(screen.getByText(`Страна:`)).toBeInTheDocument();

        expect(screen.getByText(`Слоган:`)).toBeInTheDocument();

        expect(screen.getByText(`Жанры:`)).toBeInTheDocument();

        expect(screen.getByText(`Годы выхода:`)).toBeInTheDocument();

        expect(screen.getByText(`Тип контента:`)).toBeInTheDocument();

        expect(screen.getByText(`Дата выхода:`)).toBeInTheDocument();

        expect(screen.getByText(`Бюджет:`)).toBeInTheDocument();

        expect(screen.getByText(`Возрастной рейтинг:`)).toBeInTheDocument();

        expect(screen.getByText(`IMDb: ` + mockContentData.ratings.imdbRating)).toBeInTheDocument();

        expect(screen.getByText(`Кинопоиск: ` + mockContentData.ratings.kinopoiskRating)).toBeInTheDocument();

        expect(screen.getByText(`Локальный: ` + mockContentData.ratings.localRating)).toBeInTheDocument();

        expect(screen.getByText(/В главных ролях:/)).toBeInTheDocument();
        expect(screen.getByText('Actor One, Actor Two')).toBeInTheDocument();

        expect(screen.getByText(/Также работали/)).toBeInTheDocument();
    });


    it('handles adding to favourites successfully', async () => {
        contentService.addToFavourites.mockResolvedValue({
            response: { ok: true },
            data: {},
        });

        render(<ContentInfo contentData={mockContentData} />);

        const favouriteButton = screen.getByTitle('В избранное');
        await userEvent.click(favouriteButton);

        await waitFor(() => {
            expect(contentService.addToFavourites).toHaveBeenCalledWith(mockContentData.id);
            expect(toast.success).toHaveBeenCalledWith('Добавлен в избранное', { position: 'bottom-center' });
        });
    });

    it('handles removing from favourites when add fails with 400', async () => {
        contentService.addToFavourites.mockResolvedValue({
            response: { status: 400 },
            data: {},
        });

        contentService.removeFromFavourites.mockResolvedValue({
            response: { ok: true },
            data: {},
        });

        render(<ContentInfo contentData={mockContentData} />);

        const favouriteButton = screen.getByTitle('В избранное');
        await userEvent.click(favouriteButton);

        await waitFor(() => {
            expect(contentService.addToFavourites).toHaveBeenCalledWith(mockContentData.id);
            expect(contentService.removeFromFavourites).toHaveBeenCalledWith(mockContentData.id);
            expect(toast.success).toHaveBeenCalledWith('Убран из избранных', { position: 'bottom-center' });
        });
    });

    it('handles add to favourites failure', async () => {
        contentService.addToFavourites.mockRejectedValue(new Error('Network Error'));

        render(<ContentInfo contentData={mockContentData} />);

        const favouriteButton = screen.getByTitle('В избранное');
        await userEvent.click(favouriteButton);

        await waitFor(() => {
            expect(contentService.addToFavourites).toHaveBeenCalledWith(mockContentData.id);
            expect(toast.error).toHaveBeenCalledWith('Network Error', { position: 'bottom-center' });
        });
    });

    it('renders "Нет данных" when profession has no persons', () => {
        const contentDataNoPersons = {
            ...mockContentData,
            personsInContent: [],
        };

        render(<ContentInfo contentData={contentDataNoPersons} />);

        expect(screen.getByText('Нет данных')).toBeInTheDocument();
    });

    it('renders comma-separated names correctly when less than 11 persons', () => {
        const contentDataFewPersons = {
            ...mockContentData,
            personsInContent: [
                { name: 'Actor One', profession: { professionName: 'Актер' } },
                { name: 'Actor Two', profession: { professionName: 'Актер' } },
            ],
        };

        render(<ContentInfo contentData={contentDataFewPersons} />);

        expect(screen.getByText('Actor One, Actor Two')).toBeInTheDocument();
    });

    it('renders ellipsis when there are more than or equal to 11 persons', () => {
        const manyActors = Array.from({ length: 11 }, (_, i) => ({
            name: `Actor ${i + 1}`,
            profession: { professionName: 'Актер' },
        }));
        const contentDataManyActors = {
            ...mockContentData,
            personsInContent: manyActors,
        };

        render(<ContentInfo contentData={contentDataManyActors} />);

        const actorsText = screen.getByText((content) => {
            return content.startsWith('Actor 1') && content.endsWith(', ...');
        });
        expect(actorsText).toBeInTheDocument();
    });

    it('does not render trailer section if trailerInfo is not provided', () => {
        const contentDataNoTrailer = {
            ...mockContentData,
            trailerInfo: null,
        };

        render(<ContentInfo contentData={contentDataNoTrailer} />);

        expect(screen.queryByText(mockContentData.trailerInfo?.name)).not.toBeInTheDocument();
        expect(screen.queryByTitle('trailer-iframe')).not.toBeInTheDocument();
    });

    it('displays "недостаточно оценок" when localRating is null', () => {
        const contentDataNoLocalRating = {
            ...mockContentData,
            ratings: {
                ...mockContentData.ratings,
                localRating: null,
            },
        };

        render(<ContentInfo contentData={contentDataNoLocalRating} />);

        expect(screen.getByText('Локальный: недостаточно оценок')).toBeInTheDocument();
    });

    it('renders correct pluralization for movie length', () => {
        const contentDataMovieLengthOne = {
            ...mockContentData,
            movieLength: 1,
        };

        const contentDataMovieLengthMany = {
            ...mockContentData,
            movieLength: 5, 
        };

        render(<ContentInfo contentData={contentDataMovieLengthOne} />);
        expect(screen.getByText('1 минута')).toBeInTheDocument();

        render(<ContentInfo contentData={contentDataMovieLengthMany} />);
        expect(screen.getByText('5 минут')).toBeInTheDocument();
    });

    it('does not render release date if it is not provided', () => {
        const contentDataNoReleaseDate = {
            ...mockContentData,
            releaseDate: null,
        };

        render(<ContentInfo contentData={contentDataNoReleaseDate} />);

        expect(screen.queryByText('Дата выхода:')).not.toBeInTheDocument();
    });

    it('does not render year range if it is not provided', () => {
        const contentDataNoYearRange = {
            ...mockContentData,
            yearRange: null,
        };

        render(<ContentInfo contentData={contentDataNoYearRange} />);

        expect(screen.queryByText('Годы выхода:')).not.toBeInTheDocument();
    });

    it('handles missing contentType without crashing', () => {
        const contentDataNoContentType = {
            ...mockContentData,
            contentType: null,
        };

        render(<ContentInfo contentData={contentDataNoContentType} />);

        expect(screen.queryByText('Тип контента:')).not.toBeInTheDocument();
    });
});
