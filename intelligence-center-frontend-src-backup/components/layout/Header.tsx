import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import styled from 'styled-components';
import { colors } from '../styled/StyledComponents';

const HeaderContainer = styled.header`
  background: ${colors.primary};
  color: white;
  padding: 1rem 0;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border-bottom: 1px solid ${colors.borderLight};
  width: 100%;
  z-index: 1000;
  position: relative;
`;

const NavContainer = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Logo = styled.h1`
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
  font-family: 'IBM Plex Sans', sans-serif;
  color: white;
  text-decoration: none;
`;

const Nav = styled.nav`
  display: flex;
  gap: 2rem;
`;

const NavLink = styled(Link)<{ $isActive: boolean }>`
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  transition: background-color 0.15s ease-in-out;
  font-weight: 500;
  font-family: 'IBM Plex Sans', sans-serif;

  ${props => props.$isActive && `
    background: rgba(255, 255, 255, 0.2);
  `}

  &:hover {
    background: rgba(255, 255, 255, 0.1);
    text-decoration: none;
  }
`;

const Header: React.FC = () => {
  const location = useLocation();

  return (
    <HeaderContainer style={{ backgroundColor: '#2e293d', color: 'white', padding: '1rem 0', display: 'block' }}>
      <NavContainer>
        <Logo style={{ color: 'white', fontSize: '1.5rem' }}>GlobalData Intelligence Center</Logo>        <Nav>
          <NavLink to="/" $isActive={location.pathname === '/'}>
            Home
          </NavLink>
          <NavLink to="/news" $isActive={location.pathname === '/news'}>
            News
          </NavLink>
          <NavLink to="/deals" $isActive={location.pathname === '/deals'}>
            Deals
          </NavLink>
          <NavLink to="/dashboard" $isActive={location.pathname === '/dashboard'}>
            Dashboard
          </NavLink>
        </Nav>
      </NavContainer>
    </HeaderContainer>
  );
};

export default Header;
