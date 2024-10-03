import { render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";
import { Section } from '../../../src/Pages/MainContent/components/Section';
import { MemoryRouter } from "react-router-dom";

describe('Section Component', () => {
  const sectionData = {
      name: 'Фильмы',
      contents: [{ id: 1, posterUrl: 'url1', name: 'Movie 1' }],
  };

  test('renders section with correct data', () => {
      render(<MemoryRouter><Section sectionData={sectionData} /></MemoryRouter>);

      expect(screen.getByText('Фильмы')).toBeInTheDocument();
      expect(screen.getByText('Movie 1')).toBeInTheDocument();
  });

  test('navigates to correct link on click', () => {
      render(<MemoryRouter><Section sectionData={sectionData} /></MemoryRouter>);
      const link = screen.getByText('Movie 1').closest('a');
      expect(link).toHaveAttribute('href', '/ViewContent/1');
  });
});