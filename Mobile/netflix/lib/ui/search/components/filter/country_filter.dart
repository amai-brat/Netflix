import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class CountryFilter extends StatelessWidget {
  const CountryFilter({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Column(
        spacing: 10,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('Страна', style: TextStyle(color: AppColors.textWhite, fontSize: 18, fontWeight: FontWeight.bold)),
          BlocBuilder<SearchBloc, SearchState>(
            builder: (context, state) {
              final ctx = context.read<SearchBloc>();
              final countries = state.availableCountries;
              final selectedCountry = state.filterParams.country;

              return DropdownButtonFormField<String>(
                value: selectedCountry ?? 'Не выбрано',
                dropdownColor: Colors.grey[900],
                decoration: InputDecoration(
                  border: OutlineInputBorder(),
                  filled: true,
                  fillColor: Colors.grey[800],
                ),
                items: countries.map((country) =>
                    DropdownMenuItem(
                        value: country,
                        child: Text(country,
                            style: TextStyle(color: AppColors.textWhite, fontSize: 16)))
                ).toList(),
                onChanged: (value) {
                  ctx.add(UpdateFilterParams((oldParams) =>
                      oldParams.copyWith(
                          country: (value, false)
                      )));
                },
              );
            }
          )
        ]
      ),
    );
  }
}