﻿#region copyright

// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// IEntityConfiguration.cs
//
// This file is part of JumpForJoy Software's EFCoreUtilities.
// 
// EFCoreUtilities is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// EFCoreUtilities is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with EFCoreUtilities. If not, see <https://www.gnu.org/licenses/>.

#endregion

using Microsoft.EntityFrameworkCore;

namespace J4JSoftware.EFCoreUtilities;

public interface IEntityConfiguration
{
    void Configure( ModelBuilder builder );
}

// ReSharper disable once UnusedTypeParameter
public interface IEntityConfiguration<TEntity> : IEntityConfiguration
    where TEntity : class

// ReSharper disable once RedundantTypeDeclarationBody
{
}
