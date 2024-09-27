import { beforeEach, describe, expect, test, vi } from "vitest";
import {contentService} from '../../../src/services/content.service';
import { render, screen, waitFor } from "@testing-library/react";
import { PromoSlider } from "../../../src/Pages/MainContent/components/PromoSlider";
import { MemoryRouter } from "react-router-dom";

vi.mock('../../../src/services/content.service');

describe('PromoSlider Component', () => {
  beforeEach(() => {
      vi.clearAllMocks();
  });

  test('renders promo images', async () => {
      contentService.getPromos.mockResolvedValue({
          response: { ok: true },
          data: [{id: 1, posterUrl: "url1"},
                 {id: 2, posterUrl: "url2"}
          ]
      });

      render(<MemoryRouter><PromoSlider /></MemoryRouter>);

      const promoImg = await screen.findAllByAltText('promo');
      expect(promoImg.length).toBeGreaterThan(1);
  });

  test('handles empty promo images gracefully', async () => {
      contentService.getPromos.mockResolvedValue({
          response: { ok: true },
          data: [],
      });

      render(<MemoryRouter><PromoSlider /></MemoryRouter>);

      await waitFor(() => {
        expect(screen.queryByAltText('promo')).not.toBeInTheDocument();
      });
  });
});