import { MemoryRouter } from "react-router-dom";
import Main from "../../../src/Pages/Main/Main";
import { fireEvent, render, screen } from "@testing-library/react";
import { describe, expect, test } from "vitest";

describe('Main Page', () => {
  test('renders Main component', () => {
      render(
          <MemoryRouter>
              <Main />
          </MemoryRouter>
      );

      // Check if the header elements are rendered
      expect(screen.getByText(/Неограниченное количество фильмов и сериалов/i)).toBeInTheDocument();
      expect(screen.getByRole('button', { name: /Войти/i })).toBeInTheDocument();
      expect(screen.getByRole('button', { name: /Начать/i })).toBeInTheDocument();
      expect(screen.getByText(/Часто задаваемые вопросы/i)).toBeInTheDocument();
  });

  test('renders FAQ items', () => {
      render(
          <MemoryRouter>
              <Main />
          </MemoryRouter>
      );

      // Check if FAQ items are rendered
      expect(screen.getByText(/Что такое Voltorka?/i)).toBeInTheDocument();
      expect(screen.getByText(/Что я могу смотреть в Voltorka?/i)).toBeInTheDocument();
  });

  test('toggles accordion content on title click', () => {
      render(
          <MemoryRouter>
              <Main />
          </MemoryRouter>
      );

      const accordionTitle = screen.getByText(/Что такое Voltorka?/i);
      fireEvent.click(accordionTitle);

      // Check if the content is displayed after clicking the title
      expect(screen.getByText(/Voltorka - это стриминговый сервис/i)).toBeInTheDocument();

      // Click again to close the accordion
      fireEvent.click(accordionTitle);
      expect(screen.queryByText(/Voltorka - это стриминговый сервис/i)).not.toBeInTheDocument();
  });
});
