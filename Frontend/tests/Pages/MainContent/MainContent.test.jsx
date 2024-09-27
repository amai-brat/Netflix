import {describe, vi, test, expect} from 'vitest';
import { contentService } from '../../../src/services/content.service';
import { render, screen, waitFor } from '@testing-library/react';
import MainContent from '../../../src/Pages/MainContent/MainContent';
import { MemoryRouter } from 'react-router-dom';

vi.mock('../../../src/services/content.service');
vi.mock('../../../src/Pages/MainContent/components/PromoSlider.jsx', () => {
    return {PromoSlider: () => <div id="slider"/>}
});
vi.mock('../../../src/Pages/MainContent/components/map/Map.jsx', () => {
    return {default: () => <div id="map"/>}
});

describe('MainContent Page', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    test('fetches and displays sections data', async () => {
        contentService.getSections.mockResolvedValue({
            response: { ok: true },
            data: [
                {
                    name: 'Фильмы',
                    contents: [{ id: 1, posterUrl: 'url1', name: 'Movie 1' }],
                },
                {
                    name: 'Приколы',
                    contents: [{ id: 1, posterUrl: 'url1', name: 'Movie 1' }],
                },
            ], 
        });

        render(<MemoryRouter><MainContent /></MemoryRouter>);

        await waitFor(() => {
            expect(screen.getByText(/Фильмы/i)).toBeInTheDocument();
            expect(screen.getByText(/Приколы/i)).toBeInTheDocument();
        });
    });

    test('does not render sections if API call fails', async () => {
        contentService.getSections.mockResolvedValue({
            response: { ok: false },
            data: [],
        });

        render(<MainContent />);
        
        await waitFor(() => {
            expect(screen.queryByText(/Section 1/i)).not.toBeInTheDocument();
            expect(screen.queryByText(/Section 2/i)).not.toBeInTheDocument();
        });
    });
});